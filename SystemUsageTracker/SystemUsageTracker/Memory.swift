//
//  Memory.swift
//  SystemUsageTracker
//
//  Created by Stanley Chiu on 2024-03-26.
//

import Foundation

private let HOST_VM_INFO64_COUNT: mach_msg_type_number_t =
UInt32(MemoryLayout<vm_statistics64_data_t>.size / MemoryLayout<integer_t>.size)

private let PAGE_SIZE = vm_kernel_page_size

public struct Memory{
    
    public func memoryUsage() -> (free       : Double,
                                  active     : Double,
                                  inactive   : Double,
                                  wired      : Double,
                                  compressed : Double) {
        let stats = VMStatistics64()
        
        let free        = Double(stats.free_count) * Double(PAGE_SIZE)
        let active      = Double(stats.active_count) * Double(PAGE_SIZE)
        let inactive    = Double(stats.inactive_count) * Double(PAGE_SIZE)
        let wired       = Double(stats.wire_count) * Double(PAGE_SIZE)
        let compressed  = Double(stats.compressor_page_count) * Double(PAGE_SIZE)
        
        return (free, active, inactive, wired, compressed)
    }
    
    private func VMStatistics64() -> vm_statistics64 {
        var size     = HOST_VM_INFO64_COUNT
        var hostInfo = vm_statistics64()
        
        let result = withUnsafeMutablePointer(to: &hostInfo) {
            $0.withMemoryRebound(to: integer_t.self, capacity: Int(HOST_VM_INFO64_COUNT)) {
                host_statistics64(mach_host_self(), HOST_VM_INFO64, $0, &size)
            }
        }
        
#if DEBUG
        if result != KERN_SUCCESS{
            print("Error  - \(#file): \(#function) - kern_result_t = \(result)")
        }
#endif
        
        return hostInfo
    }
}
