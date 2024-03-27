//
//  GPU.swift
//  SystemUsageTracker
//
//  Created by Stanley Chiu on 2024-03-26.
//

import Foundation
import Metal

class GPU {
    public func gpuUsage() -> Int{
        let mtlDevice = MTLCreateSystemDefaultDevice()!
        
        return mtlDevice.currentAllocatedSize
    }
}
