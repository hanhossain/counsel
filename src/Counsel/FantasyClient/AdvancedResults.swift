//
//  AdvancedResults.swift
//  Counsel
//
//  Created by Han Hossain on 6/8/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import Foundation

struct AdvancedResults {
	let quarterbacks: [AdvancedPlayerResult]?
//	let runningBacks: [AdvancedPlayerResult]?
//	let wideReceivers: [AdvancedPlayerResult]?
//	let tightEnds: [AdvancedPlayerResult]?
//	let kickers: [AdvancedPlayerResult]?
//	let defenses: [AdvancedPlayerResult]?
	
	init(json: [String : Any]) throws {
		if let quarterbacksJson = json["QB"] as? [[String : Any]] {
			var quarterbacks = [AdvancedPlayerResult]()
			
			for quarterbackJson in quarterbacksJson {
				let quarterback = try AdvancedPlayerResult(json: quarterbackJson)
				quarterbacks.append(quarterback)
			}
			self.quarterbacks = quarterbacks
		} else {
			quarterbacks = nil
		}
	}
}
