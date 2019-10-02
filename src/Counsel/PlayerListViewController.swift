//
//  PlayerListViewController.swift
//  Counsel
//
//  Created by Han Hossain on 9/29/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import UIKit

class PlayerListViewController: UITableViewController {
    
    private let cellId = "playerListCell"
    private let fantasyService = FantasyService()
    
    private var players = [Player]()
    private var sections = [(offset: Int, initial: Character)]()

    override func viewDidLoad() {
        super.viewDidLoad()
        
        fantasyService.getCurrentWeek { (result) in
            let message: String
            
            switch (result) {
            case .success((let season, let week)):
                message = "Season: \(season) - Week: \(week)"
            case .failure(let error):
                print(error.localizedDescription)
                message = "Counsel"
            }
            
            DispatchQueue.main.async {
                self.title = message
            }
        }
        
        fantasyService.getPlayers { (result) in
            switch (result) {
            case .success(let players):
                self.savePlayers(players)
                
                DispatchQueue.main.async {
                    self.tableView.reloadData()
                }
            case .failure(let error):
                print(error.localizedDescription)
            }
        }
    }
    
    private func savePlayers(_ players: [String : Player]) {
        self.players = players.values
            .sorted(by: { (player1, player2) -> Bool in
                if player1.lastName.uppercased() == player2.lastName.uppercased() {
                    return player1.firstName.uppercased() < player2.firstName.uppercased()
                }
                
                return player1.lastName.uppercased() < player2.lastName.uppercased()
            })
        
        for (index, player) in self.players.enumerated() {
            if var playerInitial = player.lastName.first?.uppercased().first {
                if playerInitial.isNumber {
                    playerInitial = "#"
                }
                
                if sections.last?.initial != playerInitial {
                    sections.append((index, playerInitial))
                }
            }
        }
    }

    // MARK: - Table view data source

    override func numberOfSections(in tableView: UITableView) -> Int {
        return sections.count
    }

    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        let sectionOffset = sections[section].offset
        let exclusiveUpperBound = section < sections.count - 1 ? sections[section + 1].offset : players.count
        return exclusiveUpperBound - sectionOffset
    }

    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: cellId, for: indexPath)
        
        let player = getPlayer(at: indexPath)
        cell.textLabel?.text = player.fullName
        cell.detailTextLabel?.text = "\(player.position) - \(player.team ?? "")"

        return cell
    }
    
    private func getPlayer(at indexPath: IndexPath) -> Player {
        let sectionOffset = sections[indexPath.section].offset
        let playerIndex = sectionOffset + indexPath.row
        return players[playerIndex]
    }
    
    override func sectionIndexTitles(for tableView: UITableView) -> [String]? {
        return sections.map { String($0.initial) }
    }

    /*
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        // Get the new view controller using segue.destination.
        // Pass the selected object to the new view controller.
    }
    */

}
