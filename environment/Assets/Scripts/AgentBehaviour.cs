﻿using UnityEngine;
using MsgPack;

[RequireComponent(typeof (AgentController))]
[RequireComponent(typeof (AgentSensor))]
public class AgentBehaviour : MonoBehaviour {
    private LISClient client = new LISClient("myagent");

    private AgentController controller;
    private AgentSensor sensor;

    private MsgPack.CompiledPacker packer = new MsgPack.CompiledPacker();

    bool created = false;

    void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Reward") {
            NotificationCenter.DefaultCenter.PostNotification(this, "OnRewardCollision");
        }
    }

    byte[] GenerateMessage() {
        Message msg = new Message();

        msg.reward = PlayerPrefs.GetFloat("Reward");
        msg.image = sensor.GetRgbImages();
        msg.depth = sensor.GetDepthImages();

        return packer.Pack(msg);
    }

    byte[] GenerateResetMessage() {
        ResetMessage msg = new ResetMessage();

        msg.reward = PlayerPrefs.GetFloat("Reward");
        msg.success = PlayerPrefs.GetInt("Success Count");
        msg.failure = PlayerPrefs.GetInt("Failure Count");
        msg.elapsed = PlayerPrefs.GetInt("Elapsed Time");

        return packer.Pack(msg);
    }

    public void Reset() {
        client.Reset(GenerateResetMessage());
    }

    void Start () {
        controller = GetComponent<AgentController>();
        sensor = GetComponent<AgentSensor>();
    }
	
    void Update () {
        if(!created) {
            if(!client.Calling) {
                client.Create(GenerateMessage());
                created = true;
            }
        } else {
            if(!client.Calling) {
                client.Step(GenerateMessage());
            }

            if(client.HasAction) {
                string action = client.GetAction();
                controller.PerformAction(action);
            }
        }
    }
}
