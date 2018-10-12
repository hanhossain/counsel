//
//  SearchViewController.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/11/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class SearchViewController: UIViewController {

	let cellId = "searchResultsCell"
	let cache = FantasyCache()
	
	var searchResults = [PlayerStatistics]()
	
	@IBOutlet weak var tableView: UITableView!
	
	override func viewDidLoad() {
        super.viewDidLoad()

        // Do any additional setup after loading the view.
    }
	
	func search(player: String) {
		cache.getPlayers(name: player) { (players) in
			guard let players = players else { return }
			
			self.searchResults = players.sorted { $0.name < $1.name }
			
			DispatchQueue.main.async {
				self.tableView.reloadData()
			}
		}
	}
}

extension SearchViewController : UITableViewDataSource {
	func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
		return searchResults.count
	}
	
	func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
		let cell = tableView.dequeueReusableCell(withIdentifier: cellId, for: indexPath)
		
		let player = searchResults[indexPath.row]
		cell.textLabel?.text = player.name
		
		return cell
	}
}

extension SearchViewController : UITextFieldDelegate {
	func textFieldShouldReturn(_ textField: UITextField) -> Bool {
		
		if let text = textField.text, !text.isEmpty {
			search(player: text)
		}
		
		textField.resignFirstResponder()
		return true
	}
}
