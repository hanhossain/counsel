//
//  MultiSelectTableViewController.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/21/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class MultiSelectTableViewController: UITableViewController {

	var data: [String]!
	
    // MARK: - Table view data source

    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return data.count
    }

    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "multiSelectCell", for: indexPath)
		
		cell.textLabel?.text = data[indexPath.row]
//		cell.accessoryType = .checkmark

        return cell
    }
	
	// MARK: - Table view delegate
	
	override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
		guard let cell = tableView.cellForRow(at: indexPath) else { return }

//		cell.accessoryType = cell.accessoryType == .none ? .checkmark : .none
		cell.setSelected(false, animated: false)
	}

}
