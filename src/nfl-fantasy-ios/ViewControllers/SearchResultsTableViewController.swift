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
		guard segue.identifier == segueToDetailController,
			let destination = segue.destination as? PlayerDetailViewController,
			let indexPath = tableView.indexPathForSelectedRow
			else { return }
		
		destination.playerStatistics = searchResults[indexPath.row]
    }
}
