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
	private var allPositions: [String]!
	
	var cache: FantasyCache!
	var delegate: SearchDelegate!
	var existingQuery: String?
	
	@IBOutlet weak var searchField: UITextField!

	@IBAction func search() {
		var query = searchField.text?.trimmingCharacters(in: .whitespacesAndNewlines)
		if query?.isEmpty == true {
			query = nil
		}
		
		delegate.search(query: query, positions: selectedPositions)
	}

	@IBAction func cancel() {
		delegate.cancel()
	}
	
	override func viewDidLoad() {
		super.viewDidLoad()
		
		searchField.text = existingQuery
		allPositions = cache.getPositions()
		selectedPositions = Set(allPositions)
	}
	
	override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
		guard let destination = segue.destination as? MultiSelectTableViewController else { return }
		
		destination.title = "Positions"
		destination.elements = allPositions
		destination.delegate = self
	}
	
}

extension SearchViewController : UITextFieldDelegate {
	
	func textFieldShouldReturn(_ textField: UITextField) -> Bool {

		textField.resignFirstResponder()
		return true
	}
}

extension SearchViewController : MultiSelectDelegate {
	
	func didSelect(_ elements: Set<String>) {
		selectedPositions = elements
	}
}
