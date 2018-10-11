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
		
		service.getLastCompletedWeek { (weekInfo, error) in
			guard let (week, season) = weekInfo else { return }
			
			print("week: \(week)")
			print("season: \(season)")
			
			service.getStatistics(season: season, week: week, completion: { (stats, error) in
				guard let stats = stats else { return }
				
				// get cam newton
				let query = stats.players.filter { $0.name.localizedCaseInsensitiveContains("newton") }
				guard let newton = query.first else { return }
				
				print(newton.name, newton.weekPoints, newton.seasonPoints)
			})
		}
	}
}
