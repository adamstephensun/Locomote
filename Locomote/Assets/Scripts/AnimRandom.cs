using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimRandom : MonoBehaviour
{
    private Animator anim;
    private float rand;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        rand = Random.Range(0, 10);
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(rand);
        anim.SetTrigger("StartAnim");
    }
}
