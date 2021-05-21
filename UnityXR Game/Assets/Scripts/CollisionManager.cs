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

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<CarterGames.Assets.AudioManager.AudioManager>();
        keyController = GameObject.Find("Keys").GetComponent<Keys>();

        rightHand = GameObject.Find("RightHand");
        rightHandPresence = rightHand.GetComponentInChildren<HandPresence>();
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision with tag: " + collision.gameObject.tag);

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

        if(collision.gameObject.tag == "StartAreaFallTrigger")
        {
            Debug.Log("Start area fall trigger");
            gameObject.transform.position = startAreaFallSpawnPoint.position;
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

        if(collision.gameObject.tag == "JetPickup")
        {
            audioManager.Play("PositiveChime", CarterGames.Assets.AudioManager.AudioHelper.AudioArgs("position", collision.gameObject.transform.position));
            //Play sound
            Debug.Log("Jet Pickup");
            rightHandPresence = rightHand.GetComponentInChildren<HandPresence>();

            if(rightHandPresence == null)
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
