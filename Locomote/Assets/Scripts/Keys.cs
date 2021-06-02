using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour
{
    public GameObject blueKey;
    public GameObject redKey;
    public GameObject greenKey;

    private bool blueAquired;
    private bool redAquired;
    private bool greenAquired;

    void Start()
    {
        blueAquired = false;
        redAquired = false;
        greenAquired = false;
    }

    public void KeyAquired(string colour)
    {
        switch(colour)
        {
            case "Blue":
                blueAquired = true;
                Destroy(blueKey);
                break;
            case "Red":
                redAquired = true;
                Destroy(redKey);
                break;
            case "Green":
                greenAquired = true;
                Destroy(greenKey);
                break;
            default:
                break;
        }

        if(blueAquired && redAquired && greenAquired)
        {
            //open the gates of babylon yea
        }
    }
}
