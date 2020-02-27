using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableRobot : MonoBehaviour
{
 private int health = 10;
 
 public void Damage(int damageAmount)
 {
     health -= damageAmount;
     Debug.Log("Fired");
     if(health <= 0)
     {
         health = 0;
         gameObject.SetActive(false);
     }
 }   
 public string get_health()
 {
     return health.ToString();
 }    
}
