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
		
		let cache = FantasyCache()
		
		cache.getPlayer(id: 2495455) { (playerStats) in
			guard let playerStats = playerStats else { return }
			
			let weeks = playerStats.weeks.values.sorted { $0.week < $1.week }
			
			print(playerStats.name)
			for week in weeks {
				print("\(week.week) \(week.points)")
			}
		}
		
		cache.getPlayers(name: "dev") { (playersStats) in
			guard let playersStats = playersStats else { return }
			
			for playerStats in playersStats {
				let weeks = playerStats.weeks.values.sorted { $0.week < $1.week }
				
				print(playerStats.name)
				for week in weeks {
					print("\(week.week) \(week.points)")
				}

			}
		}
	}
}
