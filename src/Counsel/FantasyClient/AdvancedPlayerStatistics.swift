//
//  AdvancedPlayerStatistics.swift
//  Counsel
//
//  Created by Han Hossain on 6/9/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import Foundation

struct AdvancedPlayerStatistics {
	let carries: Int?
	let fanPointsAgainstOpponentPoints: Double?
	let fanPointsAgainstOpponentRank: Double?
	let receptionPercentage: Int?
	let receptions: Int?
	let redzoneGoalToGo: Int?
	let redzoneTargets: Int?
	let redzoneTouches: Int?
	let targets: Int?
	let touches: Int?
}

extension AdvancedPlayerStatistics {
	init(json: [String: Any]) throws {
		if let carries = json["Carries"] as? String {
			self.carries = Int(carries)
		} else {
			carries = nil
		}
		
		if let fanPointsAgainstOpponentPoints = json["FanPtsAgainstOpponentPts"] as? String {
			self.fanPointsAgainstOpponentPoints = Double(fanPointsAgainstOpponentPoints)
		} else {
			fanPointsAgainstOpponentPoints = nil
		}
		
		if let fanPointsAgainstOpponentRank = json["FanPtsAgainstOpponentRank"] as? String {
			self.fanPointsAgainstOpponentRank = Double(fanPointsAgainstOpponentRank)
		} else {
			fanPointsAgainstOpponentRank = nil
		}
		
		if let receptionPercentage = json["ReceptionPercentage"] as? String {
			self.receptionPercentage = Int(receptionPercentage)
		} else {
			receptionPercentage = nil
		}
		
		if let receptions = json["Receptions"] as? String {
			self.receptions = Int(receptions)
		} else {
			receptions = nil
		}
		
		if let redzoneGoalToGo = json["RedzoneG2g"] as? String {
			self.redzoneGoalToGo = Int(redzoneGoalToGo)
		} else {
			redzoneGoalToGo = nil
		}
		
		if let redzoneTargets = json["RedzoneTargets"] as? String {
			self.redzoneTargets = Int(redzoneTargets)
		} else {
			redzoneTargets = nil
		}
		
		if let redzoneTouches = json["RedzoneTouches"] as? String {
			self.redzoneTouches = Int(redzoneTouches)
		} else {
			redzoneTouches = nil
		}
		
		if let targets = json["Targets"] as? String {
			self.targets = Int(targets)
		} else {
			targets = nil
		}
		
		if let touches = json["Touches"] as? String {
			self.touches = Int(touches)
		} else {
			touches = nil
		}
	}
}
