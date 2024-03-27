//
//  CPU.swift
//  SystemUsageTracker
//
//  Created by Stanley Chiu on 2024-03-26.
//

import Foundation

private let HOST_CPU_LOAD_INFO_COUNT: mach_msg_type_number_t =
UInt32(MemoryLayout<host_cpu_load_info_data_t>.size / MemoryLayout<integer_t>.size)

public struct CPU{
    private var loadPrevious = host_cpu_load_info()
    
    public mutating func systemUsage() -> (system : Double,
                                           user   : Double,
                                           idle   : Double,
                                           nice: Double ) {
        guard let load = hostCPULoadInfo() else {
            return (0, 0, 0, 0)
        }
        
        let userDiff = Double(load.cpu_ticks.0 - loadPrevious.cpu_ticks.0)
        let sysDiff  = Double(load.cpu_ticks.1 - loadPrevious.cpu_ticks.1)
        let idleDiff = Double(load.cpu_ticks.2 - loadPrevious.cpu_ticks.2)
        let niceDiff = Double(load.cpu_ticks.3 - loadPrevious.cpu_ticks.3)
        
        let totalTicks = sysDiff + userDiff + niceDiff + idleDiff
        
        let sys  = sysDiff  / totalTicks * 100.0
        let user = userDiff / totalTicks * 100.0
        let idle = idleDiff / totalTicks * 100.0
        let nice = niceDiff / totalTicks * 100.0
        
        loadPrevious = load
        
        return (sys, user, idle, nice)
    }
    
    private func hostCPULoadInfo() -> host_cpu_load_info? {
        var size = HOST_CPU_LOAD_INFO_COUNT
        var cpuLoadInfo = host_cpu_load_info()
        
        let result = withUnsafeMutablePointer(to: &cpuLoadInfo) {
            $0.withMemoryRebound(to: integer_t.self, capacity: Int(HOST_CPU_LOAD_INFO_COUNT)) {
                host_statistics(mach_host_self(), HOST_CPU_LOAD_INFO, $0, &size)
            }
        }
        
#if DEBUG
        if result != KERN_SUCCESS{
            print("Error  - \(#file): \(#function) - kern_result_t = \(result)")
        }
#endif
        
        return cpuLoadInfo
    }
}
