using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject levelManager = GameObject.Find("LevelManager");
        if (collision.CompareTag("Player") && levelManager != null)
        {
            levelManager.GetComponent<LevelManager>().Next();
        }
    }
}
