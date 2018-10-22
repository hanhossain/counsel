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
	
	func getPositions() -> [String] {
		let positions = Set(statsCache.values.map { $0.position })
		return positions.sorted { $0 < $1 }
	}
	
	func getTeams() -> [String] {
		let teams = Set(statsCache.values.map { $0.team })
		return teams.sorted(by: { (left, right) -> Bool in
			guard right != "" else {
				return true
			}
			
			guard left != "" else {
				return false
			}
			
			return left < right
		})
	}
	
	func getPlayer(id: Int) -> PlayerStatistics? {
		return statsCache[id]
	}
	
	func getPlayers(query: String) -> [PlayerStatistics] {
		let playerStats = statsCache.values.filter { $0.name.localizedCaseInsensitiveContains(query) }
		return playerStats
	}
	
	func getPlayers() -> [PlayerStatistics] {
		return statsCache.values.sorted { $0.name < $1.name }
	}
	
	func loadCache(completion: @escaping () -> ()) {
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
					cachedPlayer.weeks[weekStats.week] = weekStats
					statsCache[playerId] = cachedPlayer
				} else {
					let playerToCache = PlayerStatistics(id: player.id,
														 name: player.name,
														 position: player.position,
														 team: player.team,
														 weeks: [weekStats.week : weekStats])
					statsCache[playerId] = playerToCache
				}
			}
		}
	}
}
