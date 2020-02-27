using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Game : MonoBehaviour
{
    Shoot shoot;
    MQTT m_mqtt;
    Player m_player;
    ShootableRobot m_robot;
    
    
    [SerializeField] private AudioClip armorSound;
    [SerializeField] private AudioClip explodeSound;
    [SerializeField] private AudioClip surrenderSound;
    private AudioSource m_AudioSource;
    private float nextFire;
    public float fireRate = 0.25f; // number in seconds, how offen can player shoot
    public int gunDamage = 1;
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if(Time.time > nextFire){
            nextFire = Time.time + fireRate;
            string command = m_mqtt.command_received();
            bool does_hit = shoot.object_hit();
            switch (command)
            {
                case "1":
                case "3":{
                    int scale = (command=="1")?1:3;
                    nextFire = Time.time + (scale-1) * fireRate;
                    if(does_hit)
                    {   
                        m_robot = hit.collider.GetComponent<ShootableRobot>();
                        m_robot.Damage(gunDamage * scale);
                    }
                    ammo -= scale;
                    playSound(command);
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
        }


    }
}
