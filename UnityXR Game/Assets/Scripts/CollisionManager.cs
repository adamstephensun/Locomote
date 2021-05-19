using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public Transform defaultSpawnPoint;
    public Transform underMapSpawnPoint;

    private Keys keyController;

    private void Start()
    {
        keyController = GameObject.Find("Keys").GetComponent<Keys>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with tag: " + collision.gameObject.tag);

        if(collision.gameObject.tag == "UnderLevel")    //Level wide catch
        {
            gameObject.transform.position = defaultSpawnPoint.position; 
        }

        if(collision.gameObject.name == "UnderMapEnd")  //End of under map section
        {
            gameObject.transform.position = defaultSpawnPoint.position;
        }

        if(collision.gameObject.tag == "UnderMapRespawn")   //Catch in under map section
        {
            gameObject.transform.position = underMapSpawnPoint.position;
        }

        if(collision.gameObject.tag == "RedKey")
        {
            //Play sound
            Destroy(collision.gameObject);
            keyController.KeyAquired("Red");
        }

        if(collision.gameObject.tag == "BlueKey")
        {
            Debug.Log("Blue key collision");

            //Play sound
            Destroy(collision.gameObject);
            keyController.KeyAquired("Blue");

        }

        if(collision.gameObject.tag == "FuelIncreasePickup")
        {
            //play sound
            float currentMax = PlayerPrefs.GetFloat("MaxFuel");
            PlayerPrefs.SetFloat("MaxFuel", currentMax + 10);

            Debug.Log("Fuel increase pickup! Current max fuel: " + (currentMax + 10));

            Destroy(collision.gameObject);
        }

        if(collision.gameObject.tag == "BouncePlatform")
        {
            //Play bouncy sound

        }
    }
}
