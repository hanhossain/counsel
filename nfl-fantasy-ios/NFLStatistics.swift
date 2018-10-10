//
//  NFLStatistics.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/8/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import Foundation

struct NFLStatistics : Decodable {
	var statisticsType: String
	var season: String
	var week: String
	var players: [NFLPlayerStatistics]
	
	enum CodingKeys : String, CodingKey {
		case statisticsType = "statType"
		case season
		case week
		case players
	}
}
