//
//  NFLWeekStatistics.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/9/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import Foundation

struct NFLWeekStatistics : Decodable {
	var complete: Bool
	
	enum CodingKeys : String, CodingKey {
		case complete = "isWeekGamesCompleted"
	}
}
