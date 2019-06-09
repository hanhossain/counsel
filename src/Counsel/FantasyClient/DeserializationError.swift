//
//  DeserializationError.swift
//  Counsel
//
//  Created by Han Hossain on 6/8/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import Foundation

enum DeserializationError: Error {
	/// The data is corrupted or invalid
	case dataCorrupted
	
	/// The given type could not be decoded for the specified key because it did not match the type of what was found
	case typeMismatch(String)
	
	/// A non optional value was expected for the specified key, but null was found
	case valueNotFound(String)
}
