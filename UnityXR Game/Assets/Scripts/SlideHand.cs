using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideHand : MonoBehaviour
{
    private CapsuleCollider playerCollider;
    public PhysicMaterial normalPhysMat;
    public PhysicMaterial slidePhysMat;
    public PhysicMaterial gripPhysMat;

    private bool gripValue;
    private bool triggerValue;

    private MeshRenderer sphereMat;

    private Color normalCol;
    private Color slideCol;
    private Color frictionCol;


    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GameObject.Find("VRRig").GetComponent<CapsuleCollider>();
        playerCollider.material = normalPhysMat;

        sphereMat = gameObject.GetComponentInChildren<MeshRenderer>();
        sphereMat.material.DisableKeyword("_EMISSION");

        normalCol = new Color(202, 202, 202);
        slideCol = new Color(0, 247, 255);
        frictionCol = new Color(232, 65, 24);

        sphereMat.material.color = normalCol;
    }

    // Update is called once per frame
    void Update()
    {
        if (gripValue && !triggerValue)    //Slide activated
        {
            playerCollider.material = slidePhysMat;
            sphereMat.material.color = slideCol;
        }
        if(triggerValue && !gripValue)      //Grip activated
        {
            playerCollider.material = gripPhysMat;
            sphereMat.material.color = frictionCol;
        }
        if (!gripValue && !triggerValue)   //Slide deactivated
        {
            playerCollider.material = normalPhysMat;
            sphereMat.material.color = normalCol;
        }
    }

    public void updateGripValue(bool val)
    {
        gripValue = val;
    }

    public void updateTriggerValue(bool val)
    {
        triggerValue = val;
        Debug.Log("TriggerVal: " + triggerValue);
    }
}
