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
	}
}
