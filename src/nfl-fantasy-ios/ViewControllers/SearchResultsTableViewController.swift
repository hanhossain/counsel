//
//  SearchResultsTableViewController.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/13/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class SearchResultsTableViewController: UITableViewController {

	private let cellId = "searchResultsCell"
	private let segueToDetailController = "searchResultsToDetailSegue"
	private let segueToSearchController = "searchResultsToSearchSegue"
	
	private var searchResults = [PlayerStatistics]()
	
	private var filteredPositions: Set<String>!
	private var filteredTeams: Set<String>!
	
	private var query: String? {
		didSet {
			if let query = query {
				title = "Search: \"\(query)\""
			} else {
				title = nil
			}
		}
	}

	var cache: FantasyCache!
	
	@IBOutlet weak var clearButton: UIBarButtonItem!
	
	@IBAction func clearFilter(_ sender: UIBarButtonItem) {
		clear()
		
		tableView.reloadData()
	}
	
	@IBAction func search(_ sender: UIBarButtonItem) {
		modalPresentationStyle = .formSheet
		
		let playerSearchController = PlayerSearchViewController(delegate: self, cache: cache, existingPositions: filteredPositions, existingTeams: filteredTeams, existingQuery: query)
		let navigationController = UINavigationController(rootViewController: playerSearchController)
		
		present(navigationController, animated: true)
	}
	
	func clear() {
		clearButton.isEnabled = false
		
		searchResults = cache.getPlayers()
		filteredPositions = Set(cache.getPositions())
		filteredTeams = Set(cache.getTeams())
		query = nil
	}
	
	override func viewDidLoad() {
        super.viewDidLoad()
		
		clear()
    }

    // MARK: - UITableViewDataSource
	
	override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
		return searchResults.count
	}

	override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
		let cell = tableView.dequeueReusableCell(withIdentifier: cellId, for: indexPath)

		let player = searchResults[indexPath.row]
		cell.textLabel?.text = player.name
		cell.detailTextLabel?.text = "\(player.position) - \(player.team)"

		return cell
	}
	
	// MARK: - UITableViewDelegate
	
	override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
		let playerStatistics = searchResults[indexPath.row]
		let playerDetailController = PlayerDetailViewController(playerStatistics: playerStatistics)
		navigationController?.pushViewController(playerDetailController, animated: true)
	}

    // MARK: - Navigation
	
//    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
//		switch segue.identifier {
//		case segueToDetailController:
//			guard let playerDetailController = segue.destination as? PlayerDetailViewController,
//				let indexPath = tableView.indexPathForSelectedRow
//				else { return }
//
//			playerDetailController.playerStatistics = searchResults[indexPath.row]
//
//		default:
//			return
//		}
//    }
	
}

extension SearchResultsTableViewController: SearchDelegate {
	
	func search(query: String?, positions: Set<String>, teams: Set<String>) {
		
		searchResults = cache.getPlayers(query: query, positions: positions, teams: teams)
		
		self.query = query
		filteredPositions = positions
		filteredTeams = teams
		
		tableView.reloadData()
		clearButton.isEnabled = true
		
		dismiss(animated: true)
	}
	
	func cancel() {
		dismiss(animated: true)
	}
	
}
