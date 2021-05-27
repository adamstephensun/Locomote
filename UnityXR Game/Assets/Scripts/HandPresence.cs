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
            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerVal) && triggerVal > 0.1)
            {
                //Trigger pressed
                if(spawnedController.gameObject.name == "HandJet(Clone)") spawnedController.GetComponent<HandJet>().updateJetPower(triggerVal);
                else Debug.Log("No trigger input available");
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValF) && triggerVal < 0.1)
            {
                //Trigger not pressed
                if (spawnedController.gameObject.name == "HandJet(Clone)") spawnedController.GetComponent<HandJet>().updateJetPower(0);
            }

            if(targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool gripButton))
            {
                if (spawnedController.gameObject.name == "SlideHand(Clone)") spawnedController.GetComponent<SlideHand>().updateGripValue(gripButton);
                else Debug.Log("No gripButton input available");
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerButton))
            {
                if (spawnedController.gameObject.name == "SlideHand(Clone)") spawnedController.GetComponent<SlideHand>().updateTriggerValue(triggerButton);
                else Debug.Log("No triggerButton input available");
            }
        }
    }
}
