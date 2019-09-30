//
//  Player.swift
//  Counsel
//
//  Created by Han Hossain on 9/29/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import Foundation

struct Player {
    let firstName: String
    let lastName: String
    let position: String
    let active: Bool
    let id: String
    let team: String?
    
    var fullName: String {
        return "\(firstName) \(lastName)"
    }
}
