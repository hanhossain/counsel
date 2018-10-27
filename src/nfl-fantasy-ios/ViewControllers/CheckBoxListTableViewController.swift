//
//  CheckBoxListTableViewController.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/27/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class CheckBoxListTableViewController: UITableViewController {
	
	private let cellId = "checkBoxCell"
	
	var elements: [String]
	var selectedElements: [String : Bool]
	var didSelect: (Set<String>) -> ()
	
	var textForEmptyElement: String?
	
	// MARK: - Initializers
	
	convenience init(title: String, elements: [String], selectedElements: [String : Bool], didSelect: @escaping (Set<String>) -> ()) {
		self.init(title: title, elements: elements, selectedElements: selectedElements, textForEmptyElement: nil, didSelect: didSelect)
	}
	
	init(title: String, elements: [String], selectedElements: [String : Bool], textForEmptyElement: String?, didSelect: @escaping (Set<String>) -> ()) {
		
		self.elements = elements
		self.selectedElements = selectedElements
		self.didSelect = didSelect
		
		super.init(style: .grouped)
		self.title = title
	}
	
	required init?(coder aDecoder: NSCoder) {
		fatalError("init(coder:) has not been implemented")
	}
	
	// MARK: - Event Handlers
	
	@objc func selectNoElements() {
		for element in elements {
			selectedElements[element] = false
		}
		
		tableView.reloadData()
	}
	
	@objc func selectAllElements() {
		for element in elements {
			selectedElements[element] = true
		}
		
		tableView.reloadData()
	}
	
	// MARK: - UIView
	
	override func viewDidLoad() {
		super.viewDidLoad()
		
		tableView.register(UITableViewCell.self, forCellReuseIdentifier: cellId)
		
		navigationController?.setToolbarHidden(false, animated: false)
		
		let selectNone = UIBarButtonItem(title: "Select None", style: .plain, target: self, action: #selector(selectNoElements))
		let flexibleSpace = UIBarButtonItem(barButtonSystemItem: .flexibleSpace, target: nil, action: nil)
		let selectAll = UIBarButtonItem(title: "Select All", style: .plain, target: self, action: #selector(selectAllElements))
		
		toolbarItems = [selectNone, flexibleSpace, selectAll]
	}
	
	override func viewWillDisappear(_ animated: Bool) {
		super.viewWillDisappear(animated)
		
		navigationController?.setToolbarHidden(true, animated: animated)
	}
	
	override func viewDidDisappear(_ animated: Bool) {
		super.viewDidDisappear(animated)
		
		let selected = Set(selectedElements
			.filter { $0.value }
			.map { $0.key })
		
		didSelect(selected)
	}
	
	// MARK: - UITableViewDataSource
	
	override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
		return elements.count
	}
	
	override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
		let cell = tableView.dequeueReusableCell(withIdentifier: cellId, for: indexPath)
		
		let element = elements[indexPath.row]
		cell.textLabel?.text = element
		
		if element.isEmpty, let textForEmptyElement = textForEmptyElement {
			cell.textLabel?.text = textForEmptyElement
		}
		
		cell.accessoryType = selectedElements[element] == true ? .checkmark : .none
		
		return cell
	}
	
	// MARK: - UITableViewDelegate
	
	override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
		guard let cell = tableView.cellForRow(at: indexPath) else { return }
		
		let element = elements[indexPath.row]
		var isSelected = selectedElements[element] ?? false
		isSelected = !isSelected
		selectedElements[element] = isSelected
		
		cell.accessoryType = isSelected ? .checkmark : .none
		cell.setSelected(false, animated: false)
	}
}
