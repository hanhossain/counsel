//
//  NFLService.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/8/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import Foundation

class NFLService {
	
	private let decoder = JSONDecoder()
	
	func getStatistics(season: Int, week: Int, completion: @escaping (NFLStatistics?, String?) -> ()) {
		let query = "players/stats?statType=weekStats&season=\(season)&week=\(week)"
		
		makeRequest(query: query) { (stats, error) in
			completion(stats, error)
		}
	}
	
	func makeRequest<T: Decodable>(query: String, completion: @escaping (T?, String?) -> ()) {
		guard let url = URL(string: "https://api.fantasy.nfl.com/v1/\(query)") else {
			completion(nil, "url was invalid")
			return
		}
		
		URLSession.shared.dataTask(with: url) { (data: Data?, response: URLResponse?, error: Error?) in
			guard let data = data, error == nil else {
				completion(nil, "there was an attempt...")
				return
			}
			
			do {
				let result = try self.decoder.decode(T.self, from: data)
				completion(result, nil)
			} catch let err {
				completion(nil, err.localizedDescription)
			}
			
			}.resume()
	}
}
