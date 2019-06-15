//
//  FantasyService.swift
//  Counsel
//
//  Created by Han Hossain on 6/9/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import Foundation

class FantasyService {
	private let _season = 2018
	private let _numberOfWeeks = 22
	
	private var _players = [String : FantasyPlayer]()
	private var _carries = [String : [(week: Int, carries: Int?)]]() // playerId : (week, carries)
	
	var delegate: FantasyServiceDelegate?
	
	func loadCache() {
		let client = FantasyClient()
		let dispatchGroup = DispatchGroup()
		
		for week in 1..._numberOfWeeks {
			dispatchGroup.enter()
			
			client.getAdvancedResults(season: _season, week: week) { (result: Result<AdvancedResults, Error>) in
				switch result {
				case .success(let results):
					self.storeInCache(results, week: week)
				case .failure(let error):
					print(error.localizedDescription)
				}
				
				dispatchGroup.leave()
			}
		}

		if let delegate = delegate {
			dispatchGroup.notify(queue: .global(qos: .userInitiated)) {
				delegate.cacheDidLoad()
			}
		}
	}
	
	func player(id: String) -> FantasyPlayer? {
		return _players[id]
	}
	
	func getAdvancedStatistics(playerId: String) -> FantasyAdvancedStatistics? {
		guard let playerCarries = _carries[playerId] else { return nil }
		
		let sorted = playerCarries.sorted { $0.week < $1.week }
		let weeks = sorted.map { $0.week }
		let carries = sorted.map { $0.carries }
		
		let hasNoCarries = carries.allSatisfy { $0 == nil }
		return FantasyAdvancedStatistics(weeks: weeks, carries: hasNoCarries ? nil : carries)
	}
	
	func playersIds() -> [String] {
		let orderedPlayers = _players.values.sorted { $0.name < $1.name }.map { $0.id }
		return orderedPlayers
	}
	
	private func storeInCache(_ results: AdvancedResults, week: Int) {
		addToPlayersCache(results.quarterbacks, week: week)
		addToPlayersCache(results.runningBacks, week: week)
		addToPlayersCache(results.wideReceivers, week: week)
		addToPlayersCache(results.tightEnds, week: week)
		addToPlayersCache(results.kickers, week: week)
		addToPlayersCache(results.defenses, week: week)
	}
	
	private func addToPlayersCache(_ playerResults: [AdvancedPlayerResult]?, week: Int) {
		guard let playerResults = playerResults else {
			return
		}
		
		let players = playerResults.lazy.map { (playerResult) -> (FantasyPlayer, Int?) in
			let name = "\(playerResult.firstName) \(playerResult.lastName)"
			let player = FantasyPlayer(id: playerResult.id, name: name, team: playerResult.team, position: playerResult.position)
			return (player, playerResult.statistics.carries)
		}
		
		for (player, carries) in players {
			if _players[player.id] == nil {
				_players[player.id] = player
			}
			
			var playerCarries = _carries[player.id] ?? []
			playerCarries.append((week, carries))
			_carries[player.id] = playerCarries
		}
	}
}
