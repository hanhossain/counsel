//
//  AdvancedPlayerResult.swift
//  Counsel
//
//  Created by Han Hossain on 6/8/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import Foundation

struct AdvancedPlayerResult {
	let id: String
	let firstName: String
	let lastName: String
	let team: String?
	let opponentTeam: String?
	let position: Position
	let statistics: AdvancedPlayerStatistics
	let status: String?
}

extension AdvancedPlayerResult {
	init(json: [String : Any]) throws {
		guard let id = json["id"] as? String else {
			throw DeserializationError.typeMismatch("id")
		}
		self.id = id
		
		guard let firstName = json["firstName"] as? String else {
			throw DeserializationError.typeMismatch("firstName")
		}
		self.firstName = firstName
		
		guard let lastName = json["lastName"] as? String else {
			throw DeserializationError.typeMismatch("lastName")
		}
		self.lastName = lastName
		
		if let team = json["teamAbbr"] as? String {
			self.team = team.isEmpty ? nil : team
		} else {
			self.team = nil
		}
		
		if let opponentTeam = json["opponentTeamAbbr"] as? String {
			self.opponentTeam = opponentTeam == "Bye" ? nil : opponentTeam
		} else {
			self.opponentTeam = nil
		}
		
		guard let positionTemp = json["position"] as? String,
			let position = Position.init(rawValue: positionTemp) else {
				throw DeserializationError.typeMismatch("position")
		}
		self.position = position
		
		guard let statistics = json["stats"] as? [String : Any] else {
			throw DeserializationError.typeMismatch("stats")
		}
		self.statistics = try AdvancedPlayerStatistics(json: statistics)
		
		var tempStatus = json["status"] as? String
		if tempStatus?.isEmpty == true {
			tempStatus = nil
		}
		status = tempStatus
	}
}
