//
//  MultiSelectDelegate.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/21/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import Foundation

protocol MultiSelectDelegate {
	func didSelect(_ elements: Set<String>)
}
