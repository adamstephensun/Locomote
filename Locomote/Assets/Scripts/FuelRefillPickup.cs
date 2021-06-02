using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelRefillPickup : MonoBehaviour
{
    public void pickupCollected()
    {
        StartCoroutine(despawnForABit());
    }

    IEnumerator despawnForABit()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(5);
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}
