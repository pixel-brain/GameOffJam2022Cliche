using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public ParticleSystem deathParticlesPrefab;
    public CameraShake camShakeScript;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Deadly") || collision.transform.CompareTag("DeadlyTiles"))
        {
            Die();
        }
    }

    public void Die()
    {
        camShakeScript.Shake(7f, 17f, true);
        ParticleSystem deathParticles = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        DontDestroyOnLoad(deathParticles);
        GameObject.Find("LevelManager").GetComponent<LevelManager>().Die();
        Destroy(gameObject);
    }
    

}
