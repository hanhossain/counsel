//
//  ESPNClient.swift
//  Counsel
//
//  Created by Han Hossain on 9/29/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import Foundation

class ESPNClient {
    
    func getSeasons(completion: @escaping (Result<[ESPNSeason], Error>) -> ()) {
        URLSession.shared.dataTask(with: URL(string: "https://fantasy.espn.com/apis/v3/games/ffl/seasons")!) { (data, response, error) in
            guard error == nil else {
                completion(.failure(error!))
                return
            }
            
            guard let data = data else {
                completion(.failure(CounselError.invalidResponse))
                return
            }
            
            let decoder = JSONDecoder()
            
            do {
                let seasons = try decoder.decode([ESPNSeason].self, from: data)
                completion(.success(seasons))
            } catch let err {
                completion(.failure(err))
            }
        }.resume()
    }
}
