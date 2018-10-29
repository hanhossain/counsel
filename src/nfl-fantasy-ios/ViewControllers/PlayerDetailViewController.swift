//
//  PlayerDetailViewController.swift
//  nfl-fantasy-ios
//
//  Created by Han Hossain on 10/12/18.
//  Copyright © 2018 Han Hossain. All rights reserved.
//

import Charts
import UIKit

class PlayerDetailViewController: UIViewController {

	var playerStatistics: PlayerStatistics
	
	var chartView: LineChartView = {
		let lineChartView = LineChartView(frame: .zero)
		
		lineChartView.xAxis.axisMinimum = 1
		lineChartView.leftAxis.axisMinimum = 0
		lineChartView.xAxis.labelPosition = .bottom
		lineChartView.rightAxis.enabled = false
		
		return lineChartView
	}()
	
	init(playerStatistics: PlayerStatistics) {
		self.playerStatistics = playerStatistics
		
		super.init(nibName: nil, bundle: nil)
	}
	
	required init?(coder aDecoder: NSCoder) {
		fatalError("init(coder:) has not been implemented")
	}
	
	override func viewDidLoad() {
        super.viewDidLoad()
		
		title = playerStatistics.name
		view.backgroundColor = .white
		
		view.addSubview(chartView)
		
		// setup constraints for chartView
		chartView
			.constrain { $0.leadingAnchor.constraint(equalTo: view.safeAreaLayoutGuide.leadingAnchor, constant: 16) }
			.constrain { $0.trailingAnchor.constraint(equalTo: view.safeAreaLayoutGuide.trailingAnchor, constant: -16) }
			.constrain { $0.topAnchor.constraint(equalTo: view.safeAreaLayoutGuide.topAnchor, constant: 16) }
			.constrain { $0.bottomAnchor.constraint(lessThanOrEqualTo: view.bottomAnchor) }
			.constrain(withPriority: .defaultHigh) { $0.heightAnchor.constraint(equalTo: $0.widthAnchor) }
		
		displayChartData()
    }
	
	private func displayChartData() {
		let weeksStats = playerStatistics.weeks
			.values
			.sorted { $0.week < $1.week }
		
		let points = weeksStats.map { ChartDataEntry(x: Double($0.week), y: $0.points) }
		let pointsDataSet = LineChartDataSet(values: points, label: "points")
		pointsDataSet.colors = [.blue]
		
		let projectedPoints = weeksStats.map { ChartDataEntry(x: Double($0.week), y: $0.projectedPoints) }
		let projectedPointsDataSet = LineChartDataSet(values: projectedPoints, label: "projected points")
		projectedPointsDataSet.colors = [.green]
		
		chartView.data = LineChartData(dataSets: [pointsDataSet, projectedPointsDataSet])
	}

}
