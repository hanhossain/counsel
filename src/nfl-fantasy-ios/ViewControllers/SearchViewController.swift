//
//  SearchViewController.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/11/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class SearchViewController: UIViewController {
	
	var delegate: SearchDelegate!
	
	@IBOutlet weak var searchField: UITextField!
	
	@IBAction func search() {
		delegate.search(query: searchField.text)
	}
	
	@IBAction func cancel() {
		delegate.cancel()
	}
	
}

extension SearchViewController : UITextFieldDelegate {
	func textFieldShouldReturn(_ textField: UITextField) -> Bool {

		textField.resignFirstResponder()
		return true
	}
}
