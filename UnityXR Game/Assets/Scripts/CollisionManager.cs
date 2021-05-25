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
    private int fuelChangeFlag;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<CarterGames.Assets.AudioManager.AudioManager>();
        keyController = GameObject.Find("Keys").GetComponent<Keys>();

        rightHand = GameObject.Find("RightHand");
        rightHandPresence = rightHand.GetComponentInChildren<HandPresence>();

        fuelChangeFlag = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision with tag: " + collision.gameObject.tag);

        switch (collision.gameObject.tag)
        {
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
            case "FuelRefillPickup":
                PlayerPrefs.SetFloat("FuelChangeFlag", PlayerPrefs.GetFloat("MaxFuel"));
                fuelChangeFlag++;
                collision.gameObject.GetComponent<AudioSource>().Play();
                break;
            default:
                break;
        }


        if(collision.gameObject.tag == "RedKey")
        {
            collision.gameObject.GetComponent<AudioSource>().Play();
            Destroy(collision.gameObject);
            keyController.KeyAquired("Red");
        }

        if(collision.gameObject.tag == "BlueKey")
        {
            collision.gameObject.GetComponent<AudioSource>().Play();
            Destroy(collision.gameObject);
            keyController.KeyAquired("Blue");
        }

        if (collision.gameObject.tag == "GreenKey")
        {
            collision.gameObject.GetComponent<AudioSource>().Play();
            Destroy(collision.gameObject);
            keyController.KeyAquired("Green");
        }

        if (collision.gameObject.tag == "FuelIncreasePickup")
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
            collision.gameObject.GetComponent<AudioSource>().Play();
        }

        if (collision.gameObject.tag == "JetPickup")
        {
            audioManager.Play("PositiveChime", CarterGames.Assets.AudioManager.AudioHelper.AudioArgs("position", collision.gameObject.transform.position));
            //Play sound
            Debug.Log("Jet Pickup");
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
