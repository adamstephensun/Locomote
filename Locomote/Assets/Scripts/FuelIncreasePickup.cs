using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelIncreasePickup : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            float currentMax = PlayerPrefs.GetFloat("MaxFuel");
            PlayerPrefs.SetFloat("MaxFuel", currentMax + 10);

            Destroy(gameObject);
            //Play sound
        }
    }
}
