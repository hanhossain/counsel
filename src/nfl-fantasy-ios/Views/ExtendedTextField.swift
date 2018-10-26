//
//  ExtendedTextField.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/20/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class ExtendedTextField: UITextField {
	
	override func textRect(forBounds bounds: CGRect) -> CGRect {
		return createRectWithInset(forBounds: bounds)
	}
	
	override func editingRect(forBounds bounds: CGRect) -> CGRect {
		return createRectWithInset(forBounds: bounds)
	}
	
	private func createRectWithInset(forBounds bounds: CGRect) -> CGRect {
		return bounds.insetBy(dx: 16, dy: 8)
	}
}
