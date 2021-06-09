using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHandPickup : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Destroyed hand object: "+gameObject.name);
            
            Destroy(gameObject);
        }
    }
}
