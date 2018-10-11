//
//  PlayerStatistics.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/11/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import Foundation

struct PlayerStatistics {
	var id: String
	var name: String
	var position: String
	var team: String
	var weeks: [Int : WeekStatistics]
}
