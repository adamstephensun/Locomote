using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelPickupFlag : MonoBehaviour
{
    [HideInInspector]
    public bool isPickedUp;

    // Start is called before the first frame update
    void Start()
    {
        isPickedUp = false;
    }

    public void UpdateFlag()
    {
        isPickedUp = true;
    }
}
