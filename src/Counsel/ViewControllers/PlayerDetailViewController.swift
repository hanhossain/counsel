//
//  PlayerDetailViewController.swift
//  Counsel
//
//  Created by Han Hossain on 6/13/19.
//  Copyright © 2019 Han Hossain. All rights reserved.
//

import UIKit

class PlayerDetailViewController: UICollectionViewController {
	private let _cellId = "playerDetailCell"
	
	var fantasyService: FantasyService?
	var playerId: String?

    override func viewDidLoad() {
        super.viewDidLoad()

        // Register cell classes
        collectionView.register(UICollectionViewCell.self, forCellWithReuseIdentifier: _cellId)
        // Do any additional setup after loading the view.
		guard let playerId = playerId,
			let fantasyService = fantasyService else { return }
		
		guard let player = fantasyService.player(id: playerId) else { return }
		title = player.name
    }

    // MARK: UICollectionViewDataSource
	
    override func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return 1
    }

    override func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: _cellId, for: indexPath)
		
        // Configure the cell
		cell.backgroundColor = .black
    
        return cell
    }
}

extension PlayerDetailViewController: UICollectionViewDelegateFlowLayout {
	func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
		let padding: CGFloat = 8
		let totalPadding: CGFloat = padding * 2
		let width = UIScreen.main.bounds.width - totalPadding
		
		return CGSize(width: width, height: 250)
	}
}
