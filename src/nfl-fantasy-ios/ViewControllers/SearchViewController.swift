//
//  SearchViewController.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/11/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class SearchViewController: UITableViewController {
	
	private var selectedPositions: Set<String>!
	private var selectedTeams: Set<String>!
	
	private var allPositions: [String]!
	private var allTeams: [String]!
	
	var cache: FantasyCache!
	var delegate: SearchDelegate!
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
		
		selectedPositions = Set(allPositions)
		selectedTeams = Set(allTeams)
	}
	
	override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
		guard let destination = segue.destination as? MultiSelectTableViewController else { return }
		
		switch segue.identifier {
		case "positionsSegue":
			destination.didSelect = onPositionSelect(_:)
			destination.title = "Positions"
			destination.elements = allPositions
			
		case "teamsSegue":
			destination.didSelect = onTeamSelect(_:)
			destination.title = "Teams"
			destination.elements = allTeams
			
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
