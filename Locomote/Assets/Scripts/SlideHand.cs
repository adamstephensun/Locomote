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

    public ParticleSystem slideParticle;
    public ParticleSystem frictionParticle;
    private ParticleSystem.EmissionModule slideEM;
    private ParticleSystem.EmissionModule frictionEM;

    private AudioSource audio;
    private bool isAudioPlaying;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GameObject.Find("VRRig").GetComponent<CapsuleCollider>();
        playerCollider.material = normalPhysMat;
        audio = GetComponent<AudioSource>();
        audio.volume = 0.65f;

        slideEM = slideParticle.emission;
        frictionEM = frictionParticle.emission;
        slideEM.enabled = false;
        frictionEM.enabled = false;
        isAudioPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gripValue || triggerValue)   //If either input is detected
        {
            if (triggerValue && !gripValue)    //Friction activated with trigger button
            {
                playerCollider.material = slidePhysMat;
                slideEM.enabled = true;
                frictionEM.enabled = false;
            }
            if (gripValue && !triggerValue)      //Slide activated with grip button 
            {
                playerCollider.material = gripPhysMat;
                frictionEM.enabled = true;
                slideEM.enabled = false;
            }
            if(!isAudioPlaying)
            {
                audio.volume = 0.65f;
                audio.Play();
                isAudioPlaying = true;
            }
        }

        if (!gripValue && !triggerValue)   //Slide deactivated
        {
            playerCollider.material = normalPhysMat;
            frictionEM.enabled = false;
            slideEM.enabled = false;
            
            if(audio.volume > 0)
            {
                audio.volume *= 0.8f;
                if (audio.volume < 0.1f)
                {
                    isAudioPlaying = false;
                    audio.Pause();
                }
            }
        }
    }

    public void updateGripValue(bool val)
    {
        gripValue = val;
    }

    public void updateTriggerValue(bool val)
    {
        triggerValue = val;
    }
}
