//
//  FantasyCache.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/10/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import Foundation

class FantasyCache {
	
	func loadCache() {
		let service = NFLService()
		
		service.getLastCompletedWeek { (weekInfo, error) in
			guard let (week, season) = weekInfo else { return }
			
			var count = 0
			
			for weekNum in 1...week {
				service.getStatistics(season: season, week: weekNum) { (stats, error) in
					guard let stats = stats else { return }
					
					let newton = stats.players.first { $0.name.localizedCaseInsensitiveContains("newton") }
					
					let containsNewton = newton != nil
					count += 1
					
					print("week: \(stats.week) | players: \(stats.players.count) | contains newton: \(containsNewton) | count: \(count)")
				}
			}
		}
	}
}
