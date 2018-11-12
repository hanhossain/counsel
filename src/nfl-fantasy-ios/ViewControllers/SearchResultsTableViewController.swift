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

	private var searchResults = [PlayerStatistics]()

	private var filteredPositions = Set<String>()
	private var filteredTeams = Set<String>()
	
	private var cache: FantasyDataSource

	private var query: String? {
		didSet {
			if let query = query {
				title = "Search: \"\(query)\""
			} else {
				title = nil
			}
		}
	}
	
	lazy var clearButton: UIBarButtonItem = {
		return UIBarButtonItem(title: "Clear", style: .plain, target: self, action: #selector(clearFilter))
	}()
	
	lazy var searchButton: UIBarButtonItem = {
		return UIBarButtonItem(barButtonSystemItem: .search, target: self, action: #selector(searchFilter))
	}()
	
	init(_ cache: FantasyDataSource) {
		self.cache = cache
		super.init(style: .plain)
	}
	
	required init?(coder aDecoder: NSCoder) {
		fatalError("init(coder:) has not been implemented")
	}
	
	@objc func clearFilter() {
		clear()

		tableView.reloadData()
	}

	@objc func searchFilter() {
		modalPresentationStyle = .formSheet

		let playerSearchController = PlayerSearchViewController(delegate: self, cache: cache, existingPositions: filteredPositions, existingTeams: filteredTeams, existingQuery: query)
		let navigationController = UINavigationController(rootViewController: playerSearchController)

		present(navigationController, animated: true)
	}

	private func clear() {
		clearButton.isEnabled = false

		searchResults = cache.getPlayers()
		filteredPositions = Set(cache.getPositions())
		filteredTeams = Set(cache.getTeams())
		query = nil
	}

	override func viewDidLoad() {
        super.viewDidLoad()

		navigationItem.leftBarButtonItem = clearButton
		navigationItem.rightBarButtonItem = searchButton
		
		tableView.register(SubtitleTableViewCell.self, forCellReuseIdentifier: cellId)
		
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
