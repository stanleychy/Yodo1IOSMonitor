//
//  SystemUsageTracker.swift
//  SystemUsageTracker
//
//  Created by Stanley Chiu on 2024-03-26.
//

import Foundation

public class Tracker {
    
    private var timer = Timer()
    private var cpu = CPU()
    private var gpu = GPU()
    private var memory = Memory()
    
    public func StartTracking(cpuUsageDelegate: @convention(c) @escaping (Double, Double, Double, Double) -> Void,
                              gpuUsageDelegate: @convention(c) @escaping (Int) -> Void,
                              ramUsageDelegate: @convention(c) @escaping (Double, Double, Double, Double, Double) -> Void
    ) {
        timer = Timer.scheduledTimer(withTimeInterval: 1, repeats: true, block: {_ in
            let cpuUsage = self.cpu.systemUsage()
            let gpuUsage = self.gpu.gpuUsage()
            let ramUsage = self.memory.memoryUsage()
            
            cpuUsageDelegate(cpuUsage.system, cpuUsage.user, cpuUsage.idle, cpuUsage.nice)
            gpuUsageDelegate(gpuUsage)
            ramUsageDelegate(ramUsage.free, ramUsage.active, ramUsage.inactive, ramUsage.wired, ramUsage.compressed)
        })
    }
    
    public func StopTracking() {
        timer.invalidate()
    }
}
