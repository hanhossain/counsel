//
//  SearchViewController.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/11/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class SearchViewController: UITableViewController {
	
	private let positionsSection = 1
	private let teamsSection = 2
	
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
	
	// MARK: - UIView
	
	override func viewDidLoad() {
		super.viewDidLoad()
		
		searchField.text = existingQuery
		
		allPositions = cache.getPositions()
		allTeams = cache.getTeams()
		
		selectedPositions = existingPositions
		selectedTeams = existingTeams
	}
	
	// MARK: - UITableViewDelegate
	
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
			break;
			
		default:
			break;
		}
	}
}

extension SearchViewController : UITextFieldDelegate {
	
	func textFieldShouldReturn(_ textField: UITextField) -> Bool {

		textField.resignFirstResponder()
		return true
	}
}
