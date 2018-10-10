//
//  ViewController.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/8/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class ViewController: UIViewController {

	override func viewDidLoad() {
		super.viewDidLoad()
		// Do any additional setup after loading the view, typically from a nib.
		
		let service = NFLService()
		
		service.getCurrentWeek { (week, error) in
			guard let week = week, error == nil else { return }
			
			print("week: \(week)")
		}
	}
}
