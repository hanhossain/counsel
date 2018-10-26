//
//  SearchViewController.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/11/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class SearchViewController: UITableViewController {
	
	private let positionsSegue = "positionsSegue"
	private let teamsSegue = "teamsSegue"
	
	private var selectedPositions: Set<String>!
	private var selectedTeams: Set<String>!
	
	private var allPositions: [String]!
	private var allTeams: [String]!
	
	var cache: FantasyCache!
	var delegate: SearchDelegate!
	var existingPositions: Set<String>!
	var existingTeams: Set<String>!
	var existingQuery: String?
	
	@IBOutlet weak var searchField: UITextField!

	@IBAction func search() {
		var query = searchField.text?.trimmingCharacters(in: .whitespacesAndNewlines)
		if query?.isEmpty == true {
			query = nil
		}
		
		delegate.search(query: query, positions: selectedPositions, teams: selectedTeams)
	}

	@IBAction func cancel() {
		delegate.cancel()
	}
	
	func onPositionSelect(_ positions: Set<String>) {
		selectedPositions = positions
	}
	
	func onTeamSelect(_ teams: Set<String>) {
		selectedTeams = teams
	}
	
	override func viewDidLoad() {
		super.viewDidLoad()
		
		searchField.text = existingQuery
		
		allPositions = cache.getPositions()
		allTeams = cache.getTeams()
		
		selectedPositions = existingPositions
		selectedTeams = existingTeams
	}
	
	override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
		guard let destination = segue.destination as? MultiSelectTableViewController else { return }
		
		switch segue.identifier {
		case positionsSegue:
			destination.didSelect = onPositionSelect(_:)
			destination.title = "Positions"
			destination.elements = allPositions
			destination.selectedElementsMap = allPositions.reduce(into: [String : Bool]()) { (result, position) in
				result[position] = existingPositions.contains(position)
			}
			
		case teamsSegue:
			destination.didSelect = onTeamSelect(_:)
			destination.title = "Teams"
			destination.elements = allTeams
			destination.selectedElementsMap = allTeams.reduce(into: [String : Bool]()) { (result, team) in
				result[team] = existingTeams.contains(team)
			}
			destination.textForEmptyElement = "(no team)"
			
		default:
			break
		}
	}
	
}

extension SearchViewController : UITextFieldDelegate {
	
	func textFieldShouldReturn(_ textField: UITextField) -> Bool {

		textField.resignFirstResponder()
		return true
	}
}
