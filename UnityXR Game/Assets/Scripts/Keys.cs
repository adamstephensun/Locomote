using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour
{
    public GameObject blueKey;
    public GameObject redKey;

    private bool blueAquired;
    private bool redAquired;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            default:
                break;
        }

        if(blueAquired && redAquired )
        {
            //open the gates of babylon yea
        }
    }
}
