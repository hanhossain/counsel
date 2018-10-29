//
//  UIView+Extensions.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/27/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

extension UIView {
	
	@discardableResult
	func constrain(_ getConstraint: (UIView) -> (NSLayoutConstraint)) -> UIView {
		let constraint = getConstraint(self)
		constraint.isActive = true
		
		translatesAutoresizingMaskIntoConstraints = false
		
		return self
	}
	
//	@discardableResult
//	func constrain(withPriorityConstant priority: Int, constraint: (UIView) -> (NSLayoutConstraint)) -> UIView {
//		let layoutConstraint = constraint(self)
//		let originalPriority = layoutConstraint.priority.rawValue
//		let newPriority = originalPriority + Float(priority)
//
//		layoutConstraint.isActive = true
//		translatesAutoresizingMaskIntoConstraints = false
//
//		return self
//	}
	
}
