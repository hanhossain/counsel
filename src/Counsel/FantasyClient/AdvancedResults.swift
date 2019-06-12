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
	let runningBacks: [AdvancedPlayerResult]?
	let wideReceivers: [AdvancedPlayerResult]?
	let tightEnds: [AdvancedPlayerResult]?
	let kickers: [AdvancedPlayerResult]?
	let defenses: [AdvancedPlayerResult]?
}

extension AdvancedResults {
	init(json: [String : Any]) throws {
		let quarterbacksJson = json["QB"] as? [[String : Any]]
		quarterbacks = try quarterbacksJson?.map { try AdvancedPlayerResult(json: $0) }

		let runningBacksJson = json["RB"] as? [[String : Any]]
		runningBacks = try runningBacksJson?.map { try AdvancedPlayerResult(json: $0) }

		let wideReceiversJson = json["WR"] as? [[String : Any]]
		wideReceivers = try wideReceiversJson?.map { try AdvancedPlayerResult(json: $0) }

		let tightEndsJson = json["TE"] as? [[String : Any]]
		tightEnds = try tightEndsJson?.map { try AdvancedPlayerResult(json: $0) }

		let kickersJson = json["K"] as? [[String : Any]]
		kickers = try kickersJson?.map { try AdvancedPlayerResult(json: $0) }

		let defensesJson = json["DEF"] as? [[String : Any]]
		defenses = try defensesJson?.map { try AdvancedPlayerResult(json: $0) }
	}
}
