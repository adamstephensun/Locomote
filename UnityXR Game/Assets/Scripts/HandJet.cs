﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandJet : MonoBehaviour
{
    public ParticleSystem jetParticle;
    public GameObject jetNozzle;

    private Rigidbody bodyRb;
    private ParticleSystem.EmissionModule em;
    private ParticleSystem.MainModule main;

    private float startingMaxFuel;
    private float jetPower;
    private bool particleOn;
    private float currentFuel;
    private float refillTimer;

    public float powerMultiplier = 10.0f;
    public float fuelDrainRate;
    public float fuelRefillRate;
    public float refilDelay;

    private Text debugText;
    public TextMeshProUGUI fuelText;

    // Start is called before the first frame update
    void Start()
    {
        jetPower = 0;
        particleOn = false;
        em = jetParticle.emission;
        em.enabled = false;
        main = jetParticle.main;

        debugText = GameObject.Find("Debug").GetComponent<Text>();

        startingMaxFuel = 100;
        currentFuel = 100;

        PlayerPrefs.SetFloat("MaxFuel", startingMaxFuel);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(jetPower > 0.11 && currentFuel > 0)
        {
            Vector3 force = transform.up * jetPower * powerMultiplier;      //Calculates the physics force to be applied to the player

            if (bodyRb != null) bodyRb.AddForce(force);     //If there is a rigidbody, apply the force
            else bodyRb = GameObject.Find("VRRig").GetComponent<Rigidbody>();   //If there is no rb, find it

            debugText.text = "Force x:" + force.x.ToString()+ " y:" + force.y.ToString() + " z:" + force.z.ToString();

            refillTimer = 0;    //Resets the fuel refil timer when the jet is powered
            decreaseFuel();     //Decreases the fuel level

            main.startSpeed = 4 + jetPower; //Changes the speed of the particle system based on the power

            if (particleOn == false && currentFuel > 0)
            {
                particleOn = true;
                em.enabled = true;
                jetNozzle.GetComponent<Renderer>().material.color = Color.red;
            }
        }
        if (jetPower == 0)
        {
            refillTimer += Time.deltaTime;
            if (refillTimer > refilDelay) increaseFuel();

            em.enabled = false;
            particleOn = false;
            jetNozzle.GetComponent<Renderer>().material.color = Color.black;
        }

        float currentMax = PlayerPrefs.GetFloat("MaxFuel");
        string fuelString = "Fuel: " + (int)currentFuel + "/" + (int)currentMax;
        fuelText.SetText(fuelString);
    }

    public void updateJetPower(float val)
    {
        jetPower = val;
    }

    public void setRBReference(Rigidbody rb)
    {
        bodyRb = rb;
    }

    private void increaseFuel()
    {
        float currentMax = PlayerPrefs.GetFloat("MaxFuel");
        if(currentFuel < currentMax)   //If fuel isnt full
        {
            currentFuel += fuelRefillRate;  //Increase fuel by the refill rate
            if (currentFuel >= currentMax) currentFuel = currentMax;  //If the current fuel is >= the max fuel, set fuel to max fuel
        }
    }

    private void decreaseFuel()
    {
        if (currentFuel > 0) //If there is fuel in the tank
        {
            currentFuel -= jetPower;   //Reduce the fuel by the amount of power being used plus the drain rate
            if (currentFuel <= 0) currentFuel = 0;  //If the current fuel is <= 0, set it to zero
        }
    }
}