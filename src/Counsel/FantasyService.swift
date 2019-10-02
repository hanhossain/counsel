//
//  FantasyService.swift
//  Counsel
//
//  Created by Han Hossain on 9/29/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import Foundation

class FantasyService {
    private let espnClient = ESPNClient()
    private let sleeperClient = SleeperClient()
    
    private var players = [String : SleeperPlayer]()
    
    func getCurrentWeek(completion: @escaping (Result<(season: Int, week: Int), Error>) -> ()) {
        espnClient.getSeasons { (espnResult) in
            switch (espnResult) {
            case .success(let seasons):
                guard let latestSesason = seasons.first else {
                    completion(.failure(CounselError.invalidResponse))
                    return
                }
                
                completion(.success((latestSesason.id, latestSesason.currentScoringPeriod.id)))
            case .failure(let error):
                completion(.failure(error))
            }
        }
    }
    
    func getPlayers(completion: @escaping (Result<[String : Player], Error>) -> ()) {
        guard players.isEmpty else {
            completion(.success(self.getPlayers()))
            return
        }
        
        sleeperClient.getPlayers { (result) in
            switch (result) {
            case .success(let players):
                self.saveActivePlayers(players)
                completion(.success(self.getPlayers()))
                return
            case .failure(let error):
                completion(.failure(error))
                return
            }
        }
    }
    
    private func saveActivePlayers(_ players: [String : SleeperPlayer]) {
        let validPositions: Set = ["QB", "RB", "WR", "TE", "K", "DEF"]
        let activePlayers = players.values.filter { $0.active && validPositions.contains($0.position ?? "") }.map { ($0.id, $0) }
        self.players = Dictionary(uniqueKeysWithValues: activePlayers)
    }
    
    private func getPlayers() -> [String : Player] {
        return self.players.mapValues { Player(firstName: $0.firstName,
                                               lastName: $0.lastName,
                                               position: $0.position ?? "",
                                               active: $0.active,
                                               id: $0.id,
                                               team: $0.team) }
    }
}
