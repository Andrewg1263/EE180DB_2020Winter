using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] public Text ammoField; 
    [SerializeField] public Text Health;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip anti_bioticSound;
    MQTT m_mqtt;
    Shoot m_shoot;
    
    [SerializeField] int m_ammo;
    [SerializeField] int m_health;
    // Start is called before the first frame update
    void Start()
    {
        m_mqtt = gameObject.GetComponentInParent<MQTT>();
    }
    public void damage(int d){
        m_health -= d;
    }

    public int player_ammo(){
        return m_ammo;
    }
    public int player_health(){
        return m_health;
    }
    void Update()
    {
        string command = m_mqtt.command_received();
        bool does_hit = m_shoot.object_hit();
        switch(command)
        {
            case "1":
            case "3"
        }
    }
}
