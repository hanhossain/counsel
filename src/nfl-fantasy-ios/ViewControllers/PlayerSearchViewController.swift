//
//  PlayerSearchViewController.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/28/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class PlayerSearchViewController: UITableViewController {
	
	private let searchCellId = "searchCell"
	private let dropdownCellId = "dropdownCell"
	private let searchFieldSection = 0
	private let positionsSection = 1
	private let teamsSection = 2
	
	private var selectedPositions: Set<String>
	private var selectedTeams: Set<String>
	private var allPositions: [String]
	private var allTeams: [String]
	
	var delegate: SearchDelegate
	var cache: FantasyDataSource
	var existingPositions: Set<String>
	var existingTeams: Set<String>
	var existingQuery: String?
	
	lazy var cancelButton: UIBarButtonItem = {
		return UIBarButtonItem(barButtonSystemItem: .cancel, target: self, action: #selector(cancel))
	}()

	lazy var searchButton: UIBarButtonItem = {
		return UIBarButtonItem(title: "Search", style: .plain, target: self, action: #selector(search))
	}()
	
	// MARK: - Initializers
	
	init(delegate: SearchDelegate, cache: FantasyDataSource, existingPositions: Set<String>, existingTeams: Set<String>, existingQuery: String?) {
		self.delegate = delegate
		self.cache = cache
		self.existingPositions = existingPositions
		self.existingTeams = existingTeams
		self.existingQuery = existingQuery
		
		// TODO: do I need to store the existing positions/teams if I have a selected variable?
		self.selectedPositions = existingPositions
		self.selectedTeams = existingTeams
		
		self.allPositions = cache.getPositions()
		self.allTeams = cache.getTeams()
		
		super.init(style: .grouped)
	}
	
	required init?(coder aDecoder: NSCoder) {
		fatalError("init(coder:) has not been implemented")
	}
	
	// MARK: - Event Handlers
	
	@objc func cancel() {
		delegate.cancel()
	}
	
	@objc func search() {
		
		let indexPath = IndexPath(row: 0, section: searchFieldSection)
		let searchFieldCell = tableView.cellForRow(at: indexPath) as? SearchFieldTableViewCell
		var query = searchFieldCell?.textField.text?.trimmingCharacters(in: .whitespacesAndNewlines)
		
		if query?.isEmpty == true {
			query = nil
		}

		delegate.search(query: query, positions: selectedPositions, teams: selectedTeams)
	}
	
	// MARK: - UIView
	
	override func viewDidLoad() {
		super.viewDidLoad()
		
		navigationItem.leftBarButtonItem = cancelButton
		navigationItem.rightBarButtonItem = searchButton
		
		// register cells
		tableView.register(SearchFieldTableViewCell.self, forCellReuseIdentifier: searchCellId)
		tableView.register(DropdownTableViewCell.self, forCellReuseIdentifier: dropdownCellId)
	}
	
	// MARK: - UITableViewDataSource
	
	override func numberOfSections(in tableView: UITableView) -> Int {
		return 3
	}
	
	override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
		return 1
	}
	
	override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
		
		var cell: UITableViewCell
		
		switch indexPath.section {
		case 1:
			cell = tableView.dequeueReusableCell(withIdentifier: dropdownCellId, for: indexPath)
			cell.textLabel?.text = "Positions"

		case 2:
			cell = tableView.dequeueReusableCell(withIdentifier: dropdownCellId, for: indexPath)
			cell.textLabel?.text = "Teams"
			
		default:
			cell = tableView.dequeueReusableCell(withIdentifier: searchCellId, for: indexPath)
			
			let searchCell = cell as? SearchFieldTableViewCell
			searchCell?.textField.text = existingQuery
		}
		
		return cell
	}
	
	// MARK: - UITableViewDelegate
	
	override func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
		return 44
	}
	
	override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
		switch indexPath.section {
		case positionsSection:
			let selectedElements = allPositions.reduce(into: [String : Bool]()) { (result, position) in
				result[position] = existingPositions.contains(position)
			}
			
			let checkBoxListTableViewController = CheckBoxListTableViewController(title: "Position", elements: allPositions, selectedElements: selectedElements) { positions in
				self.selectedPositions = positions
			}
			
			navigationController?.pushViewController(checkBoxListTableViewController, animated: true)
			
		case teamsSection:
			let selectedElements = allTeams.reduce(into: [String : Bool]()) { (result, team) in
				result[team] = existingTeams.contains(team)
			}
			
			let checkBoxListTableViewController = CheckBoxListTableViewController(title: "Teams", elements: allTeams, selectedElements: selectedElements, textForEmptyElement: "(no team)") { teams in
				self.selectedTeams = teams
			}
			
			navigationController?.pushViewController(checkBoxListTableViewController, animated: true)
			
		default:
			break
		}
	}
}
