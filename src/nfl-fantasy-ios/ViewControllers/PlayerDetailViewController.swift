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

	var playerStatistics: PlayerStatistics!
	
	@IBOutlet weak var chartView: LineChartView!
	
	override func viewDidLoad() {
        super.viewDidLoad()
		
		title = playerStatistics.name
		
		// display chart data
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
		
		chartView.xAxis.axisMinimum = 1
		chartView.leftAxis.axisMinimum = 0
		chartView.xAxis.labelPosition = .bottom
		chartView.rightAxis.enabled = false
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
