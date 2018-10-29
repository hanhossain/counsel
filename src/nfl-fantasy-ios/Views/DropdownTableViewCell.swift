//
//  DropdownTableViewCell.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/28/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class DropdownTableViewCell: UITableViewCell {
	
	override init(style: UITableViewCell.CellStyle, reuseIdentifier: String?) {
		super.init(style: style, reuseIdentifier: reuseIdentifier)
		
		accessoryType = .disclosureIndicator
	}
	
	required init?(coder aDecoder: NSCoder) {
		super.init(coder: aDecoder)
	}
	
}
