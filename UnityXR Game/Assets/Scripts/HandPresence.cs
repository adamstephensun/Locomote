using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/*
    Controller input controller
    Reference for input names can be found here https://docs.unity3d.com/Manual/xr_input.html#XRInputMappings
 */

public class HandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;

    public GameObject emptyHandPrefab;
    public GameObject leftControllerPrefab;
    public bool isRight;

    private InputDevice targetDevice;

    private GameObject spawnedController;

    // Start is called before the first frame update
    void Start()
    {
        TryInitialiseHands();
    }

    public void TryInitialiseHands()
    {
        List<InputDevice> devices = new List<InputDevice>();        //Create and populate a list of the current devices
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (spawnedController != null) Destroy(spawnedController);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];

            GameObject prefab;
            if (isRight) prefab = emptyHandPrefab;     //If the hand presence prefab is right, set the prefab to the right hand
            else prefab = leftControllerPrefab;

            if (prefab) spawnedController = Instantiate(prefab, transform);         //If a prefab is loaded, instantiate it
            else spawnedController = Instantiate(leftControllerPrefab, transform);  //If the current prefab is empty for some reason, use the left hand prefab as the default
            
            spawnedController.GetComponent<HandJet>().setRBReference(GameObject.Find("VRRig").GetComponent<Rigidbody>()); //Get a reference of the body rb to the hand jet
        }
    }

    void Update()
    {
        if(!targetDevice.isValid)       //If the device is not valid, try again
        {
            TryInitialiseHands();
        }
        else
        {
            if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonVal) && primaryButtonVal)
            {
                //Primary button (X/A) pressed
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValF) && !primaryButtonVal)
            {
                //Primary button not pressed
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerVal) && triggerVal > 0.1)
            {
                //Trigger pressed
                if(spawnedController.gameObject.name == "HandJet(Clone)") spawnedController.GetComponent<HandJet>().updateJetPower(triggerVal);
                else {
                    Debug.Log("HandJet not equipped");
                }
            }

            if(targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripVal) && gripVal > 0.1)
            {
                if (spawnedController.gameObject.name == "HandJet(Clone)") spawnedController.GetComponent<SlideHand>().updateGripValue(gripVal);
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValF) && triggerVal < 0.1)
            {
                //Trigger not pressed
                spawnedController.GetComponent<HandJet>().updateJetPower(0);
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisVal) && primary2DAxisVal != Vector2.zero)
            {
                //Right thumbstick moved
            }
        }
    }
}
