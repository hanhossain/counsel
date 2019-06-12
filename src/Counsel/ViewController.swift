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
		
		let client = FantasyClient()
		client.getAdvancedResults(season: 2018, week: 2) { (result) in
			switch result {
			case .success(let value):
				if let count = value.quarterbacks?.count {
					print(count)
				}
				
				let playerCarries = value.quarterbacks?.compactMap({ (playerResult: AdvancedPlayerResult) -> (name: String, carries: Int)? in
					if let carries = playerResult.statistics.carries {
						let name = "\(playerResult.firstName) \(playerResult.lastName)"
						return (name, carries)
					}
					return nil
				})
				
				if let playerCarries = playerCarries {
					for player in playerCarries {
						print("\(player.name): \(player.carries)")
					}
				}
				
			case .failure(let error):
				print(error.localizedDescription)
			}
		}
	}
}

