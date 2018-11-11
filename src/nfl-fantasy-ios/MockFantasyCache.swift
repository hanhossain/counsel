//
//  MockFantasyCache.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 11/11/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import Foundation

class MockFantasyCache: FantasyDataSource {
	private var player: PlayerStatistics = {
		let week1 = WeekStatistics(week: 1, points: 21.4, projectedPoints: 15.6)
		let week2 = WeekStatistics(week: 2, points: 15.0, projectedPoints: 10)
		let weeks = [ 1 : week1, 2: week2]
		return PlayerStatistics(id: "1111", name: "Billy Joe", position: "P1", team: "Team1", weeks: weeks)
	}()
	
	func getPositions() -> [String] {
		return ["P1", "P2", "P3"]
	}
	
	func getTeams() -> [String] {
		return ["Team1", "Team2", "Team3"]
	}
	
	func getPlayer(id: Int) -> PlayerStatistics? {
		return player
	}
	
	func getPlayers(query: String?, positions: Set<String>, teams: Set<String>) -> [PlayerStatistics] {
		return [player]
	}
	
	func getPlayers() -> [PlayerStatistics] {
		return [player]
	}
	
	func loadCache(completion: @escaping () -> ()) {
		completion()
	}
	
	
}
