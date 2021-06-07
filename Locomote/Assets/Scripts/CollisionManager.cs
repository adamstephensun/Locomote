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

    public AudioClip pickupClip;
    public AudioClip dieClip;
    public AudioClip successClip;

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
                AudioSource.PlayClipAtPoint(dieClip, gameObject.transform.position);
                break;
            case "UnderMapEnd":     //End of under map section
                gameObject.transform.position = defaultSpawnPoint.position;
                AudioSource.PlayClipAtPoint(successClip, gameObject.transform.position);
                break;
            case "UnderMapRespawn": //Catch in under map section
                gameObject.transform.position = underMapSpawnPoint.position;
                AudioSource.PlayClipAtPoint(dieClip, gameObject.transform.position);
                break;
            case "StartAreaFallTrigger":    //Catch if user falls down tube in the start area
                gameObject.transform.position = startAreaFallSpawnPoint.position;
                AudioSource.PlayClipAtPoint(dieClip, gameObject.transform.position);
                break;
            #endregion 
            #region pickups
            /////////////---Pickups---//////////////
            case "RedKey":      //Red key pickup
                AudioSource.PlayClipAtPoint(pickupClip, other.gameObject.transform.position);

                keyController.KeyAquired("Red");
                Destroy(other.gameObject);
                break;
            case "BlueKey":     //Blue key pickup
                AudioSource.PlayClipAtPoint(pickupClip, other.gameObject.transform.position);

                keyController.KeyAquired("Blue");
                Destroy(other.gameObject);
                break;
            case "GreenKey":    //Green key pickup
                AudioSource.PlayClipAtPoint(pickupClip, other.gameObject.transform.position);

                keyController.KeyAquired("Green");
                Destroy(other.gameObject);
                break;
            case "FuelIncreasePickup":      //Max fuel increase pickup

                if(!other.gameObject.GetComponent<FuelPickupFlag>().isPickedUp)
                {
                    other.gameObject.GetComponent<FuelPickupFlag>().UpdateFlag();
                    AudioSource.PlayClipAtPoint(pickupClip, other.gameObject.transform.position);

                    float currentMax = PlayerPrefs.GetFloat("MaxFuel");
                    PlayerPrefs.SetFloat("MaxFuel", currentMax + 10);
                    Destroy(other.gameObject);
                }

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
            AudioSource.PlayClipAtPoint(pickupClip, collision.gameObject.transform.position);

            rightHandPresence = rightHand.GetComponentInChildren<HandPresence>();
            if (rightHandPresence == null)
            {
                Debug.Log("Could not find right hand presence");
                rightHandPresence = rightHand.GetComponentInChildren<HandPresence>();
            }

            rightHandPresence.emptyHandPrefab = jetPrefab;
            rightHandPresence.TryInitialiseHands();

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "SlidePickup")
        {
            Debug.Log("Slide pickup");
            AudioSource.PlayClipAtPoint(pickupClip, collision.gameObject.transform.position);

            leftHandPresence = leftHand.GetComponentInChildren<HandPresence>();
            if (leftHandPresence == null)
            {
                Debug.Log("Could not find left hand presence");
                leftHandPresence = leftHand.GetComponentInChildren<HandPresence>();
            }

            leftHandPresence.leftControllerPrefab = slideHandPrefab;
            leftHandPresence.TryInitialiseHands();

            Destroy(collision.gameObject);
        }
    }
}
