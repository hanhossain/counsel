//
//  SubtitleTableViewCell.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 11/6/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import UIKit

class SubtitleTableViewCell: UITableViewCell {
	
	override init(style: UITableViewCell.CellStyle, reuseIdentifier: String?) {
		super.init(style: .subtitle, reuseIdentifier: reuseIdentifier)
	}
	
	required init?(coder aDecoder: NSCoder) {
		fatalError("init(coder:) has not been implemented")
	}
	
}
