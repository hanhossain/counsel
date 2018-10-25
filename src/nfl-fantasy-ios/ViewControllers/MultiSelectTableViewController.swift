//
//  MultiSelectTableViewController.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/21/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class MultiSelectTableViewController: UITableViewController {

	private var selectedElementsMap = [String : Bool]()
	
	var elements: [String]!
	
	var didSelect: ((Set<String>) -> ())!
	
	@IBAction func selectNone() {
		for element in elements {
			selectedElementsMap[element] = false
		}
		
		tableView.reloadData()
	}
	
	@IBAction func selectAll() {
		for element in elements {
			selectedElementsMap[element] = true
		}
		
		tableView.reloadData()
	}
	
	// MARK: - UIViewController
	
	override func viewDidLoad() {
		super.viewDidLoad()
		
		navigationController?.setToolbarHidden(false, animated: true)
		
		for element in elements {
			selectedElementsMap[element] = true
		}
	}
	
	override func viewWillDisappear(_ animated: Bool) {
		super.viewWillDisappear(animated)
		
		navigationController?.setToolbarHidden(true, animated: true)
	}
	
	override func viewDidDisappear(_ animated: Bool) {
		super.viewDidDisappear(animated)
		
		let selectedElements = Set(selectedElementsMap
			.filter { $0.value }
			.map { $0.key })
		
		didSelect(selectedElements)
	}
	
    // MARK: - UITableViewDataSource

    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return elements.count
    }

    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "multiSelectCell", for: indexPath)
		
		let element = elements[indexPath.row]
		cell.textLabel?.text = element
		cell.accessoryType = selectedElementsMap[element] == true ? .checkmark : .none

        return cell
    }
	
	// MARK: - UITableViewDelegate
	
	override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
		guard let cell = tableView.cellForRow(at: indexPath) else { return }

		let element = elements[indexPath.row]
		var isSelected = selectedElementsMap[element] ?? false
		isSelected = !isSelected
		selectedElementsMap[element] = isSelected
		
		cell.accessoryType = isSelected ? .checkmark : .none
		cell.setSelected(false, animated: false)
	}

}
