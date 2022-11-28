using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    public ParticleSystem triggeredParticles;
    public bool toMenu;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject levelManager = GameObject.Find("LevelManager");
        if (collision.CompareTag("Player") && levelManager != null)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/LevelSFX/LevelFinish");
            triggeredParticles.Play();
            levelManager.GetComponent<LevelManager>().Next(toMenu);
        }
    }
}
