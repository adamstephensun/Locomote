using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CollisionManager : MonoBehaviour
{
    private Keys keyController;

    private GameObject rightHand;
    private GameObject leftHand;
    private HandPresence rightHandPresence;
    private HandPresence leftHandPresence;

    public GameObject jetPrefab;
    public GameObject slideHandPrefab;

    public Transform defaultSpawnPoint;
    public Transform underMapSpawnPoint;
    public Transform startAreaFallSpawnPoint;

    [HideInInspector]
    public bool fuelChangeFlag;

    private void Start()
    {
        keyController = GameObject.Find("Keys").GetComponent<Keys>();

        rightHand = GameObject.Find("RightHand");
        rightHandPresence = rightHand.GetComponentInChildren<HandPresence>();

        leftHand = GameObject.Find("LeftHand");
        leftHandPresence = leftHand.GetComponentInChildren<HandPresence>();

        fuelChangeFlag = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            #region respawnTriggers
            //////////---Respawn triggers---///////////
            case "UnderLevel":      //Level wide catch
                gameObject.transform.position = defaultSpawnPoint.position;
                break;
            case "UnderMapEnd":     //End of under map section
                gameObject.transform.position = defaultSpawnPoint.position;
                break;
            case "UnderMapRespawn": //Catch in under map section
                gameObject.transform.position = underMapSpawnPoint.position;
                break;
            case "StartAreaFallTrigger":    //Catch if user falls down tube in the start area
                gameObject.transform.position = startAreaFallSpawnPoint.position;
                break;
            #endregion 
            #region pickups
            /////////////---Pickups---//////////////
            case "RedKey":      //Red key pickup
                other.gameObject.GetComponent<AudioSource>().Play();

                keyController.KeyAquired("Red");
                Destroy(other.gameObject);
                break;
            case "BlueKey":     //Blue key pickup
                other.gameObject.GetComponent<AudioSource>().Play();

                keyController.KeyAquired("Blue");
                Destroy(other.gameObject);
                break;
            case "GreenKey":    //Green key pickup
                other.gameObject.GetComponent<AudioSource>().Play();

                keyController.KeyAquired("Green");
                Destroy(other.gameObject);
                break;
            case "FuelIncreasePickup":      //Max fuel increase pickup
                other.gameObject.GetComponent<AudioSource>().Play();

                float currentMax = PlayerPrefs.GetFloat("MaxFuel");
                PlayerPrefs.SetFloat("MaxFuel", currentMax + 10);
                Destroy(other.gameObject);
                break;
            case "FuelRefillPickup":        //Fuel refill ring pickup
                other.gameObject.GetComponent<AudioSource>().Play();

                fuelChangeFlag = true;
                other.gameObject.GetComponent<FuelRefillPickup>().pickupCollected();
                break;
                #endregion
        }


    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision with tag: "+collision.gameObject.tag);

        switch (collision.gameObject.tag)
        {
            ////////////---Level---////////////
            case "BouncePlatform":      //Yellow bounce platforms
                collision.gameObject.GetComponent<AudioSource>().Play();
                break;
            default:
                break;
        }

        if (collision.gameObject.tag == "JetPickup")
        {
            collision.gameObject.GetComponent<AudioSource>().Play();

            if (rightHandPresence == null)
            {
                Debug.Log("Could not find right hand presence");
                rightHandPresence = rightHand.GetComponentInChildren<HandPresence>();
            }

            rightHandPresence.GetComponent<HandPresence>().emptyHandPrefab = jetPrefab;
            rightHandPresence.TryInitialiseHands();

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "SlidePickup")
        {
            Debug.Log("Slide pickup");
            collision.gameObject.GetComponent<AudioSource>().Play();

            if (leftHandPresence == null)
            {
                Debug.Log("Could not find left hand presence");
                leftHandPresence = leftHand.GetComponentInChildren<HandPresence>();
            }

            leftHandPresence.GetComponent<HandPresence>().leftControllerPrefab = slideHandPrefab;
            leftHandPresence.TryInitialiseHands();

            Destroy(collision.gameObject);
        }
    }
}
