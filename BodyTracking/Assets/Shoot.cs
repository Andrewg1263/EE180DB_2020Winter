// shoot script, fire, check if it's hit
// return boolean value with function object_hit()
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using System;
[RequireComponent(typeof(AudioSource))]


public class Shoot : MonoBehaviour
{
    public GameObject gameCamera;
    public GameObject MQTT;
    public float weaponRange = 15000f;	
    public string command;
    public int ammo = 20;
    private bool ObjectHit; 

    [SerializeField] private readonly int gunDamage = 1;
    // private MQTT mqttScript;
    private float rate = 3f; 
    private float nextTime = 0;
    private float distance = 0;
    
    RaycastHit hit;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip chargedShotSound;
    private AudioSource m_AudioSource;

    //MQTT
	private MqttClient subscriber;
	string IP_address = "131.179.0.191";
    //MQTT initializ end
    void Start()
    {   
        ammoField = GameObject.Find("Canvas/Ammo").GetComponent<Text>();
        ammoField.text = "Ammo: 3";
        Health = GameObject.Find("Canvas/Health").GetComponent<Text>();

        // MQTT starts
        // create subscriber instance 
		subscriber = new MqttClient(IPAddress.Parse(IP_address),1883 , false , null ); 
		
		// register to message received 
		subscriber.MqttMsgPublishReceived += client_MqttMsgPublishReceived; 
		
		string clientId = Guid.NewGuid().ToString(); 
		subscriber.Connect(clientId); 
		
		// subscribe to the topic "/home/temperature" with QoS 2 
		subscriber.Subscribe(new string[] { "hello/world" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }); 
        //MQTT ends
        audioSource = GetComponent<AudioSource>();
        
        Debug.Log("Started");
    }
    int armorLeft = 0;
    void Update() {
        ammoField.text = ("Ammo: "+ (ammo.ToString()));
        bool detected = Physics.Raycast(gameCamera.transform.position, gameCamera.transform.forward, out hit);
        Debug.Log("Found an object - distance: " + hit.distance.ToString());
        distance = hit.distance;

        switch (command)
        {
            case "1":
            case "3":{
                if(ammo > 0 && Time.time > nextTime){
                    int scale = (command=="1")?1:3;
                    nextTime = Time.time + scale*rate;
                    if(detected && distance < 5)
                    {   
                        if(armorLeft > 0){
                            armorLeft -= scale;
                            Health.text = ("Health: "+ robot.get_health()+ " (+" + armorLeft.ToString() + ")");
                        }
                        else{
                            robot = hit.collider.GetComponent<ShootableRobot>();
                            robot.Damage(gunDamage*scale);
                            Health.text = ("Health: "+ robot.get_health());
                        }
                        
                    }
                    ammo -= scale;
                    playSound(command);
                }
                break;
            }
            case "2":{
                if(ammo <= 5)
                {
                    ammo = 20;
                }
                playSound(command);
                break;
            }
            case "4":{
                armorLeft = 5;
                Health.text = ("Health: "+ robot.get_health()+ " (+" + armorLeft.ToString() + ")");
                playSound(command);
                break;
            }
            case "5":{
                playSound(command);
                break;
            }
            case "6":{
                playSound(command);
                break;
            }
            case "7":{
                robot.Damage(1000000);
                playSound(command);
                break;
            }
            default:
                break;
        }
        command = "0";
        distance = 0;

    }

    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
	{ 
		command = System.Text.Encoding.UTF8.GetString(e.Message);
		Debug.Log("Received: " + command);
	} 

    private void playSound(string sel)
    {
        // change audio source
        switch (sel){
            // shot
            case "1": 
                audioSource.clip = fireSound;
                break;
            // reload
            case "2":
                audioSource.clip = reloadSound;
                break;
            // supercharge
            case "3":
                audioSource.clip = chargedShotSound;
                break;
            // armor
            case "4":
                audioSource.clip = armorSound;
                break;
            // explode
            case "5":
                audioSource.clip = explodeSound;
                break;
            // anti-biotic
            case "6":
                audioSource.clip = anti_bioticSound;
                break;
            // surrender
            case "7":
                audioSource.clip = surrenderSound;
                break;    
            default:
                break;
        }
        audioSource.Play();
    }
    
    public bool object_hit(){
        return ObjectHit;
    }

}
