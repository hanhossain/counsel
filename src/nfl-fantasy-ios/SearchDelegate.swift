//
//  SearchDelegate.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/13/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import Foundation

protocol SearchDelegate {
	
	func search(query: String?, positions: Set<String>)

	func cancel()
}
