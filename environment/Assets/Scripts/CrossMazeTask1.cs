﻿using UnityEngine;
using System;

public class CrossMazeTask1 : CrossMazeTaskBase {
    string automation;

    public override string AutomationSequence() { return automation; }

    float rewardValue = 2.0F;

    public override string Name() { return "Cross Maze Task 1"; }

    public override void Initialize(int success, int failure) {
        // 仕様「S地点は3ヶ所あり、ランダムにスタート地点が決定する」を実現する

        int phase = (int)(UnityEngine.Random.value * 3);
        float x = 0.0f;
        float y = 1.12f;
        float z = 0.5f;

        float rx = 0.0f;
        float ry = 0.0f;
        float rz = 0.0f;
        Quaternion rotation = Quaternion.identity;
			
        switch(phase) {
            case 0:
                // 南端からスタート {0, 0, 0}
                automation = new String('2', 11);
                break;
            case 1:
                // 東端からスタート {0, 0, 0}
                automation = String.Join("", new string[] {
                    new String('2', 6),
                    new String('0', 9),
                    new String('2', 5)
                });
                x = 12.0f;
                z = 12.5f;
                ry = -90.0f;
                break;
            case 2:
                // 西端からスタート {0, 0, 0}
                automation = String.Join("", new string[] {
                    new String('2', 6),
                    new String('1', 9),
                    new String('2', 5)
                });
                x = -12.0f;
                z = 12.5f;
                ry = 90.0f;
                break;
            default:
                break;
        }

        agent.transform.position = new Vector3(x, y, z);
        rotation.eulerAngles = new Vector3 (rx, ry, rz);
        agent.transform.rotation = rotation;
    }

    public override bool Success() {
        return rewardCount > 0;
    }

    void FixedUpdate() {
        Punishment();
        rewardValue -= 0.002F;
        if(rewardValue < 0.0F) {
            rewardValue = 0.0F;
        }
    }
}
