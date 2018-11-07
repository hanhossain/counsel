//
//  SearchFieldTableViewCell.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/27/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class SearchFieldTableViewCell: UITableViewCell {
	
	var textField: ExtendedTextField = {
		let field = ExtendedTextField()
		field.placeholder = "Player"
		field.clearButtonMode = .always
		return field
	}()
	
	override init(style: UITableViewCell.CellStyle, reuseIdentifier: String?) {
		super.init(style: style, reuseIdentifier: reuseIdentifier)
		
		contentView.addSubview(textField)
		
		textField
			.constrain { $0.leadingAnchor.constraint(equalTo: contentView.leadingAnchor) }
			.constrain { $0.trailingAnchor.constraint(equalTo: contentView.trailingAnchor) }
			.constrain { $0.topAnchor.constraint(equalTo: contentView.topAnchor) }
			.constrain { $0.bottomAnchor.constraint(equalTo: contentView.bottomAnchor) }
		
		textField.delegate = self
	}
	
	required init?(coder aDecoder: NSCoder) {
		fatalError("init(coder:) has not been implemented")
	}
}

extension SearchFieldTableViewCell: UITextFieldDelegate {
	func textFieldShouldReturn(_ textField: UITextField) -> Bool {
		
		textField.resignFirstResponder()
		return true
	}
}
