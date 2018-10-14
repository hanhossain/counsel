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
	}
	
	
	override func viewDidLoad() {
        super.viewDidLoad()

        // Do any additional setup after loading the view.
    }
	
}

//extension SearchViewController : UITextFieldDelegate {
//	func textFieldShouldReturn(_ textField: UITextField) -> Bool {
//
//		if let text = textField.text, !text.isEmpty {
//			search(query: text)
//		}
//
//		textField.resignFirstResponder()
//		return true
//	}
//}
