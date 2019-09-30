//
//  FantasyService.swift
//  Counsel
//
//  Created by Han Hossain on 9/29/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import Foundation

class FantasyService {
    let espnClient = ESPNClient()
    
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
}
