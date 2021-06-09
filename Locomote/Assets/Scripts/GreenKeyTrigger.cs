using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKeyTrigger : MonoBehaviour
{
    public GameObject wall;
    public AudioClip wallClip;

    public void OpenWall()
    {
        Debug.Log("Trigger key");
        AudioSource.PlayClipAtPoint(wallClip, wall.transform.position);
        wall.SetActive(false);
    }
}
