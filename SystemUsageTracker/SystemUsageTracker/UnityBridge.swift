//
//  UnityBridge.swift
//  SystemUsageTracker
//
//  Created by Stanley Chiu on 2024-03-26.
//

import Foundation

let tracker = Tracker()

@_cdecl("StartTracking")
public func StartTracking(cpuUsageDelegate: @convention(c) @escaping (Double, Double, Double, Double) -> Void,
                          gpuUsageDelegate: @convention(c) @escaping (Int) -> Void,
                          ramUsageDelegate: @convention(c) @escaping (Double, Double, Double, Double, Double) -> Void) {
    tracker.StartTracking(cpuUsageDelegate: cpuUsageDelegate,
                          gpuUsageDelegate: gpuUsageDelegate,
                          ramUsageDelegate: ramUsageDelegate)
}

@_cdecl("StopTracking")
public func StopTracking() {
    tracker.StopTracking()
}
