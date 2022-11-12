using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float destroyTime;
    public bool destroyOnCollision;
    public ParticleSystem destroyParticles;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (destroyOnCollision)
        {
            if (destroyParticles != null)
            {
                ParticleSystem particles = Instantiate(destroyParticles, transform.position, Quaternion.identity);
                transform.right = (Vector3)collision.contacts[0].point - transform.position;
            }
            Destroy(gameObject);
        }
    }
}
