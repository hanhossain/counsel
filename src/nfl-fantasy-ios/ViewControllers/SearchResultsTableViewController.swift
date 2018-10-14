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

	var cache: FantasyCache!
	
    override func viewDidLoad() {
        super.viewDidLoad()
		
		searchResults = cache.getPlayers()
    }

    // MARK: - Table view data source
	
	override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
		return searchResults.count
	}

	override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
		let cell = tableView.dequeueReusableCell(withIdentifier: cellId, for: indexPath)

		let player = searchResults[indexPath.row]
		cell.textLabel?.text = player.name

		return cell
	}

    // MARK: - Navigation
	
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
		switch segue.identifier {
		case segueToDetailController:
			guard let destination = segue.destination as? PlayerDetailViewController,
				let indexPath = tableView.indexPathForSelectedRow
				else { return }
			
			destination.playerStatistics = searchResults[indexPath.row]
		
		case segueToSearchController:
			let destination = segue.destination as? SearchViewController
			destination?.delegate = self
			
		default:
			return
		}
    }
	
}

extension SearchResultsTableViewController: SearchDelegate {
	
	func search(query: String?) {
		
		if let query = query, !query.isEmpty {
			searchResults = cache.getPlayers(query: query)
			
			title = "Search: \"\(query)\""
			tableView.reloadData()
		}
		
		dismiss(animated: true)
	}
	
	func cancel() {
		dismiss(animated: true)
	}
	
}
