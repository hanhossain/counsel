//
//  PlayerListViewController.swift
//  Counsel
//
//  Created by Han Hossain on 6/11/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import UIKit

class PlayerListViewController: UITableViewController, FantasyServiceDelegate {
	private let _cellId = "playerListCell"
	private let _fantasyService = FantasyService()
	private let _showPlayerDetailSegue = "showPlayerDetail"
	
	private var _sectionIdPlayerIdMap = [Character : [String]]()
	private var _sectionIds = [Character]()
	
	override func viewDidLoad() {
		_fantasyService.delegate = self
		_fantasyService.loadCache()
	}
	
	override func numberOfSections(in tableView: UITableView) -> Int {
		return _sectionIds.count
	}
	
	override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
		let sectionId = _sectionIds[section]
		return _sectionIdPlayerIdMap[sectionId]?.count ?? 0
	}
	
	override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
		let cell = tableView.dequeueReusableCell(withIdentifier: _cellId, for: indexPath)
		
		if let playerId = playerId(for: indexPath) {
			
			if let player = _fantasyService.player(id: playerId) {
				cell.textLabel?.text = player.name
				let position = player.position.rawValue
				let team = player.team ?? .empty
				cell.detailTextLabel?.text = "\(position) - \(team)"
			}
			
		}
		
		return cell
	}
	
	override func sectionIndexTitles(for tableView: UITableView) -> [String]? {
		return _sectionIds.map { String($0) }
	}
	
	override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
		if segue.identifier == _showPlayerDetailSegue {
			if let destination = segue.destination as? PlayerDetailViewController {
				destination.fantasyService = _fantasyService
				if let indexPath = tableView.indexPathForSelectedRow {
					destination.playerId = playerId(for: indexPath)
				}
			}
		}
	}
	
	func cacheDidLoad() {
		print("the cache loaded")
		
		let players = _fantasyService.playersIds()
		
		for playerId in players {
			if let player = _fantasyService.player(id: playerId) {
				if let firstLetter = player.name.first {
					if _sectionIds.last != firstLetter {
						_sectionIds.append(firstLetter)
						_sectionIdPlayerIdMap[firstLetter] = []
					}
					_sectionIdPlayerIdMap[firstLetter]?.append(playerId)
				}
			}
		}
		
		DispatchQueue.main.async {
			self.tableView.reloadData()
		}
	}
	
	private func playerId(for indexPath: IndexPath) -> String? {
		let sectionId = _sectionIds[indexPath.section]
		return _sectionIdPlayerIdMap[sectionId]?[indexPath.row]
	}
}
