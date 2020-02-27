using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using System;

public class MQTT : MonoBehaviour {

	[SerializeField] private string command;
	private MqttClient subscriber;
	// Use this for initialization
	string IP_address = "192.168.31.243";
	void Start () {
		// create subscriber instance 
		subscriber = new MqttClient(IPAddress.Parse(IP_address),1883 , false , null ); 
		
		// register to message received 
		subscriber.MqttMsgPublishReceived += client_MqttMsgPublishReceived; 
		
		string clientId = Guid.NewGuid().ToString(); 
		subscriber.Connect(clientId); 
		
		// subscribe to the topic "/home/temperature" with QoS 2 
		subscriber.Subscribe(new string[] { "hello/world" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }); 

	}
	void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
	{ 
		command = System.Text.Encoding.UTF8.GetString(e.Message);
		Debug.Log("Received: " + command);
	} 

	public string command_received(){
		return command;
	}
	void Update () {
	}
}
