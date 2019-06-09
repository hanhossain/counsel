//
//  ViewController.swift
//  Counsel
//
//  Created by Han Hossain on 6/8/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import UIKit

class ViewController: UIViewController {

	override func viewDidLoad() {
		super.viewDidLoad()
		// Do any additional setup after loading the view.
	}
	
	@IBAction func getData(_ sender: UIButton) {
		print("hello world")
		
		let client = FantasyClient()
		client.getAdvancedResults(season: 2018, week: 2) { (result) in
			switch result {
			case .success(let value):
				if let quarterbacks = value.quarterbacks {
					for qb in quarterbacks {
						print("\(qb.firstName) \(qb.lastName)")
					}
				}
			case .failure(let error):
				print(error.localizedDescription)
			}
		}
	}
}

