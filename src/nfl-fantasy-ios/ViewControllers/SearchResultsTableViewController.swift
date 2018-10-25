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
		clearButton.isEnabled = false
		searchResults = cache.getPlayers()
		query = nil
		
		tableView.reloadData()
	}
	
	override func viewDidLoad() {
        super.viewDidLoad()
		
		clearButton.isEnabled = false
		
		searchResults = cache.getPlayers()
		filteredPositions = Set(cache.getPositions())
		filteredTeams = Set(cache.getTeams())
    }

    // MARK: - Table view data source
	
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

    // MARK: - Navigation
	
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
		switch segue.identifier {
		case segueToDetailController:
			guard let playerDetailController = segue.destination as? PlayerDetailViewController,
				let indexPath = tableView.indexPathForSelectedRow
				else { return }

			playerDetailController.playerStatistics = searchResults[indexPath.row]

		case segueToSearchController:
			let navigationController = segue.destination as? UINavigationController
			if let searchController = navigationController?.topViewController as? SearchViewController {
				searchController.delegate = self
				searchController.cache = cache
				searchController.existingQuery = query
				searchController.existingPositions = filteredPositions
				searchController.existingTeams = filteredTeams
			}
		default:
			return
		}
    }
	
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
