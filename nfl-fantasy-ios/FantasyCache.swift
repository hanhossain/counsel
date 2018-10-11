//
//  FantasyCache.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/10/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import Foundation

class FantasyCache {
	
	private var statsCache = [Int : PlayerStatistics]() // { id, playerStats }
	private var playerNameToIdMap = [String : Int]() // { name, id }
	
	func getStatistics(id: Int, completion: @escaping (PlayerStatistics?, String?) -> ()) {
		// if the cache has been loaded, get the player from the cache
		// otherwise, load the cache
		if statsCache.count > 0 {
			guard let playerStats = statsCache[id] else {
				completion(nil, "could not find player with id \(id)")
				return
			}
			
			completion(playerStats, nil)
		} else {
			loadCache {
				guard let playerStats = self.statsCache[id] else {
					completion(nil, "could not find player with id \(id)")
					return
				}
				
				completion(playerStats, nil)
			}
		}
	}
	
	private func loadCache(completion: @escaping () -> ()) {
		let service = NFLService()
		
		service.getLastCompletedWeek { (weekInfo, error) in
			guard let (numberOfWeeks, season) = weekInfo else { return }
			
			var listOfStats = [NFLStatistics]()
			
			for weekNum in 1...numberOfWeeks {
				service.getStatistics(season: season, week: weekNum) { (stats, error) in
					guard let stats = stats else { return }
					
					listOfStats.append(stats)
					
					if listOfStats.count == numberOfWeeks {
						self.storeInCache(listOfStats)
						completion()
					}
				}
			}
		}
	}
	
	private func storeInCache(_ statsList: [NFLStatistics]) {
		for stats in statsList {
			guard let week = Int(stats.week) else { return }
			
			for player in stats.players {
				guard let playerId = Int(player.id) else { return }
				let weekStats = WeekStatistics(week: week, points: player.weekPoints, projectedPoints: player.weekProjectedPoints)
				
				// store player's week in cache
				if var cachedPlayer = statsCache[playerId] {
					cachedPlayer.weeks.append(weekStats)
					statsCache[playerId] = cachedPlayer
				} else {
					let playerToCache = PlayerStatistics(id: player.id,
														 name: player.name,
														 position: player.position,
														 team: player.team,
														 weeks: [weekStats])
					statsCache[playerId] = playerToCache
				}
				
				// make sure player name to id map is updated
				playerNameToIdMap[player.name] = playerId
			}
		}
	}
}
