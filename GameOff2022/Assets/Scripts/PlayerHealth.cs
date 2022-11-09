using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public ParticleSystem deathParticlesPrefab;
    public ParticleSystem invinsibleParticles;
    public float invinsibleTime = 7f;
    float invinsibleTimer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy Stuff
        if (invinsibleTimer > 0)
        {
            if (collision.transform.CompareTag("DeadlyTiles"))
            {
                Vector2 hitPosition;
                Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();
                foreach (ContactPoint2D hit in collision.contacts)
                {
                    hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                    hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                    tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
                }
            }
            else if (collision.transform.CompareTag("Deadly"))
            {
                Destroy(collision.gameObject);
            }
        }
        // Die to stuff
        else
        {
            if (collision.transform.CompareTag("Deadly") || collision.transform.CompareTag("DeadlyTiles"))
            {
                Die();
            }
        }
    }

    private void Update()
    {
        if (invinsibleTimer > 0)
        {
            if (!invinsibleParticles.isPlaying)
                invinsibleParticles.Play();
        }
        else
        {
            invinsibleParticles.Stop();
        }


        invinsibleTimer -= Time.deltaTime;
    }

    void Die()
    {
        ParticleSystem deathParticles = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        DontDestroyOnLoad(deathParticles);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BecomeInvinsible()
    {
        invinsibleTimer = invinsibleTime;
    }
}
