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
	
	private var _players = [String]()
	
	override func viewDidLoad() {
		_fantasyService.delegate = self
		_fantasyService.loadCache()
	}
	
	override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
		return _players.count
	}
	
	override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
		let cell = tableView.dequeueReusableCell(withIdentifier: _cellId, for: indexPath)
		
		if let player = _fantasyService.player(id: _players[indexPath.row]) {
			cell.textLabel?.text = player.name
			let position = player.position.rawValue
			let team = player.team ?? .empty
			cell.detailTextLabel?.text = "\(position) - \(team)"
		}
		
		return cell
	}
	
	func cacheDidLoad() {
		print("the cache loaded")
		
		_players = _fantasyService.playersIds()
		
		DispatchQueue.main.async {
			self.tableView.reloadData()
		}
	}
}
