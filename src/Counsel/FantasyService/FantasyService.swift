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
	
	var delegate: FantasyServiceDelegate?
	
	func loadCache() {
		let client = FantasyClient()
		let dispatchGroup = DispatchGroup()
		
		for week in 1..._numberOfWeeks {
			dispatchGroup.enter()
			
			client.getAdvancedResults(season: _season, week: week) { (result: Result<AdvancedResults, Error>) in
				switch result {
				case .success(let results):
					self.storeInCache(results)
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
	
	func playersIds() -> [String] {
		let orderedPlayers = _players.values.sorted { $0.name < $1.name }.map { $0.id }
		return orderedPlayers
	}
	
	private func storeInCache(_ results: AdvancedResults) {
		addToPlayersCache(results.quarterbacks)
		addToPlayersCache(results.runningBacks)
		addToPlayersCache(results.wideReceivers)
		addToPlayersCache(results.tightEnds)
		addToPlayersCache(results.kickers)
		addToPlayersCache(results.defenses)
	}
	
	private func addToPlayersCache(_ playerResults: [AdvancedPlayerResult]?) {
		guard let playerResults = playerResults else {
			return
		}
		
		let players = playerResults.lazy.map { (playerResult) -> FantasyPlayer in
			let name = "\(playerResult.firstName) \(playerResult.lastName)"
			let player = FantasyPlayer(id: playerResult.id, name: name, team: playerResult.team, position: playerResult.position)
			return player
		}
		
		for player in players {
			if _players[player.id] == nil {
				_players[player.id] = player
			}
		}
	}
}
