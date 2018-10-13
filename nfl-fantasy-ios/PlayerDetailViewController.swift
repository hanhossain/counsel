//
//  PlayerDetailViewController.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/12/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class PlayerDetailViewController: UIViewController {

	var playerStatistics: PlayerStatistics!
	
    override func viewDidLoad() {
        super.viewDidLoad()

        // Do any additional setup after loading the view.
		print(playerStatistics.name)
    }
    

    /*
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        // Get the new view controller using segue.destination.
        // Pass the selected object to the new view controller.
    }
    */

}
