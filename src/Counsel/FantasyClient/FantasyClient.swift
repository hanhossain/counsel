//
//  FantasyClient.swift
//  Counsel
//
//  Created by Han Hossain on 6/8/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import Foundation

class FantasyClient {
	
	private let _baseAddress = "https://api.fantasy.nfl.com/v1/players"
	
	func getAdvancedResults(season: Int, week: Int, completion: @escaping (Result<AdvancedResults, Error>) -> Void) {
		let path = "advanced?format=json&season=\(season)&week=\(week)&count=\(Int.max)"
		makeRequest(path: path, completion: completion, createResponse: { try AdvancedResults(json: $0) })
	}
	
	private func makeRequest<T>(path: String, completion: @escaping (Result<T, Error>) -> Void, createResponse: @escaping ([String : Any]) throws -> T ) {
		guard let url = URL(string: "\(_baseAddress)/\(path)") else {
			completion(.failure(URLError(URLError.badURL)))
			return
		}
		
		URLSession.shared.dataTask(with: url) { (data, response, error) in
			if let error = error {
				completion(.failure(error))
				return
			}
			
			guard let response = response as? HTTPURLResponse else {
				// it will always be a HTTPURLResponse
				return
			}
			
			if response.statusCode == 500 {
				completion(.failure(FantasyClientError.internalServerError))
				return
			}
			
			if let data = data {
				guard response.statusCode == 200 else {
					let errorsJson = try? JSONSerialization.jsonObject(with: data) as? [String : [[String : String]]]
					let errors = errorsJson?["errors"] ?? []
					completion(.failure(FantasyClientError.badRequest(errors)))
					return
				}
				
				guard let json = try? JSONSerialization.jsonObject(with: data) as? [String : Any] else {
					completion(.failure(DeserializationError.dataCorrupted))
					return
				}
				
				let result = Result { try createResponse(json) }
				completion(result)
			}
		}.resume()
	}
}
