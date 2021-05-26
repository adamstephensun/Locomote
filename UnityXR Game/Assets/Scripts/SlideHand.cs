using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideHand : MonoBehaviour
{
    private CapsuleCollider playerCollider;
    public PhysicMaterial normalPhysMat;
    public PhysicMaterial slidePhysMat;

    private bool gripValue;

    private MeshRenderer sphereMat;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GameObject.Find("VRRig").GetComponent<CapsuleCollider>();
        playerCollider.material = normalPhysMat;

        sphereMat = gameObject.GetComponentInChildren<MeshRenderer>();
        sphereMat.material.DisableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        if (gripValue)    //Slide activated
        {
            playerCollider.material = slidePhysMat;
            sphereMat.material.EnableKeyword("_EMISSION");
           
        }
        if (!gripValue)   //Slide deactivated
        {
            playerCollider.material = normalPhysMat;
            sphereMat.material.DisableKeyword("_EMISSION");
        }
    }

    public void updateGripValue(bool val)
    {
        gripValue = val;
    }
}
