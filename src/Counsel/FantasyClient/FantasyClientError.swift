//
//  FantasyClientError.swift
//  Counsel
//
//  Created by Han Hossain on 6/8/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import Foundation

enum FantasyClientError: Error {
	case badRequest([[String : String]])
	case internalServerError
}

extension FantasyClientError: LocalizedError {
	var errorDescription: String? {
		switch self {
		case .badRequest(let errors):
			return "Bad request: \(errors)"
			
		case .internalServerError:
			return "Internal server error"
		}
	}
}
