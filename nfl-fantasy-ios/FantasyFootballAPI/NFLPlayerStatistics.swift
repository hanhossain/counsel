//
//  NFLPlayerStatistics.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/9/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import Foundation

struct NFLPlayerStatistics : Decodable {
	var id: String
	var name: String
	var position: String
	var team: String
	var seasonPoints: Double
	var seasonProjectedPoints: Double
	var weekPoints: Double
	var weekProjectedPoints: Double
	
	enum CodingKeys : String, CodingKey {
		case id
		case name
		case position
		case team = "teamAbbr"
		case seasonPoints = "seasonPts"
		case seasonProjectedPoints = "seasonProjectedPts"
		case weekPoints = "weekPts"
		case weekProjectedPoints = "weekProjectedPts"
	}
}
