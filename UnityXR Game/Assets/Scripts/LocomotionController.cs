using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour       //Controller for the teleportation locomotion. On the VRRig
{
    public XRController leftTeleRay;        //Reference to the teleport ray on the left hand
    public XRController rightTeleRay;       //"                                  " right hand  
    public InputHelpers.Button teleportActivationButton;    //The button that the user presses to teleport
    public float activationThreshold = 0.1f;                //The threshold for the button press being recognised. Used for triggers with float inputs

    private ContinuousMovement movement;

    private void Start()
    {
        movement = GameObject.Find("VRRig").GetComponent<ContinuousMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(movement.GroundCheck())
        {
            if (leftTeleRay)
            {
                leftTeleRay.gameObject.SetActive(CheckIfActivated(leftTeleRay));
            }
            if (rightTeleRay)
            {
                rightTeleRay.gameObject.SetActive(CheckIfActivated(rightTeleRay));
            }
        }
    }

    public bool CheckIfActivated(XRController controller)       //Returns a bool to determine if the tele ray is activated
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}
