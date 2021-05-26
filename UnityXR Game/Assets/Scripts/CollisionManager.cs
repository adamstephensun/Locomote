using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CollisionManager : MonoBehaviour
{
    public Transform defaultSpawnPoint;
    public Transform underMapSpawnPoint;
    public Transform startAreaFallSpawnPoint;

    private Keys keyController;

    private GameObject rightHand;
    private HandPresence rightHandPresence;
    public GameObject jetPrefab;

    private CarterGames.Assets.AudioManager.AudioManager audioManager;

    [HideInInspector]
    public bool fuelChangeFlag;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<CarterGames.Assets.AudioManager.AudioManager>();
        keyController = GameObject.Find("Keys").GetComponent<Keys>();

        rightHand = GameObject.Find("RightHand");
        rightHandPresence = rightHand.GetComponentInChildren<HandPresence>();

        fuelChangeFlag = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
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

                /////////////---Pickups---//////////////
            case "RedKey":      //Red key pickup
                collision.gameObject.GetComponent<AudioSource>().Play();
                Destroy(collision.gameObject);
                keyController.KeyAquired("Red");
                break;
            case "BlueKey":     //Blue key pickup
                collision.gameObject.GetComponent<AudioSource>().Play();
                Destroy(collision.gameObject);
                keyController.KeyAquired("Blue");
                break;
            case "GreenKey":    //Green key pickup
                collision.gameObject.GetComponent<AudioSource>().Play();
                Destroy(collision.gameObject);
                keyController.KeyAquired("Green");
                break;
            case "FuelIncreasePickup":      //Max fuel increase pickup
                float currentMax = PlayerPrefs.GetFloat("MaxFuel");
                collision.gameObject.GetComponent<AudioSource>().Play();
                PlayerPrefs.SetFloat("MaxFuel", currentMax + 10);
                Destroy(collision.gameObject);
                break;
            case "FuelRefillPickup":        //Fuel refill ring pickup
                fuelChangeFlag = true;
                collision.gameObject.GetComponent<AudioSource>().Play();
                collision.gameObject.GetComponent<FuelRefillPickup>().pickupCollected();
                break;

            ////////////---Level---////////////
            case "BouncePlatform":      //Yellow bounce platforms
                collision.gameObject.GetComponent<AudioSource>().Play();
                break;
            default:
                break;
        }

        if (collision.gameObject.tag == "JetPickup")
        {
            audioManager.Play("PositiveChime", CarterGames.Assets.AudioManager.AudioHelper.AudioArgs("position", collision.gameObject.transform.position));
            rightHandPresence = rightHand.GetComponentInChildren<HandPresence>();

            if (rightHandPresence == null)
            {
                Debug.Log("Could not find right hand presence");
                rightHandPresence = rightHand.GetComponentInChildren<HandPresence>();
            }

            rightHandPresence.GetComponent<HandPresence>().emptyHandPrefab = jetPrefab;
            rightHandPresence.TryInitialiseHands();

            Destroy(collision.gameObject);
        }
    }
}
