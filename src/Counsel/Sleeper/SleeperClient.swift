//
//  SleeperClient.swift
//  Counsel
//
//  Created by Han Hossain on 9/29/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import Foundation

class SleeperClient {
    
    func getPlayers(completion: @escaping (Result<[String : SleeperPlayer], Error>) -> ()) {
        let url = URL(string: "https://api.sleeper.app/v1/players/nfl")!
        URLSession.shared.dataTask(with: url) { (data, response, error) in
            guard error == nil else {
                completion(.failure(error!))
                return
            }

            guard let data = data else {
                completion(.failure(CounselError.invalidResponse))
                return
            }
            
            do {
                let decoder = JSONDecoder()
                let players = try decoder.decode([String : SleeperPlayer].self, from: data)
                completion(.success(players))
            } catch let err {
                completion(.failure(err))
            }
        }.resume()
    }
}
