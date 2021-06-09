using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Keys keyController;

    public Animator doorAnim;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(keyController.allKeys)
            {
                doorAnim.SetTrigger("open");
            }
        }
    }
}
