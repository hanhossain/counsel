//
//  ESPNSeason.swift
//  Counsel
//
//  Created by Han Hossain on 9/29/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import Foundation

struct ESPNSeason: Codable {
    let id: Int
    let active: Bool
    let currentScoringPeriod: ESPNCurrentScoringPeriod
}

struct ESPNCurrentScoringPeriod: Codable {
    let id: Int
}
