//
//  FantasyDataSource.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 11/11/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import Foundation

protocol FantasyDataSource {
	func getPositions() -> [String]
	func getTeams() -> [String]
	func getPlayer(id: Int) -> PlayerStatistics?
	func getPlayers(query: String?, positions: Set<String>, teams: Set<String>) -> [PlayerStatistics]
	func getPlayers() -> [PlayerStatistics]
	func loadCache(completion: @escaping () -> ())
}
