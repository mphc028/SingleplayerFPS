using System;
using UnityEngine;

public class FootstepController : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerController player;
    public AudioSourceArray[] footstepSources;


    public float footstepInterval = 0.1f;

    [SerializeField] private AudioSource audioSource;
    private float nextFootstepTime = 1f;

    private float count = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (player.onGrounded && count >= nextFootstepTime)
        {
            if (rb.linearVelocity.magnitude <= 2f && rb.linearVelocity.magnitude > 1f)
            {
                PlayRandomFootstepSound();
                nextFootstepTime = 1 / (footstepInterval * (rb.linearVelocity.magnitude*1.5f));
                count = 0;
            }
            if (rb.linearVelocity.magnitude > 2f)
            {
                PlayRandomFootstepSound();
                nextFootstepTime = 1 / (footstepInterval * (rb.linearVelocity.magnitude));
                count = 0;
            }

        }
        else if (count < nextFootstepTime)
        {
            count += Time.deltaTime;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 40, 100, 20), "count: " + nextFootstepTime);
    }

    private int getSourceIndex()
    {
        switch(player.groundType)
        {
            case "water": return 1;
            default: return 0; 
        }
    }

    private void PlayRandomFootstepSound()
    {
        AudioClip[] footstepSounds = footstepSources[getSourceIndex()].AudioClips;
        if (footstepSounds.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, footstepSounds.Length);
            audioSource.PlayOneShot(footstepSounds[randomIndex]);
        }
    }
}