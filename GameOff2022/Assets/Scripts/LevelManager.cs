using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    int current;
    public static bool created;

    void Awake()
    {
        if (created)
        {
            Destroy(gameObject);
        }
        else
        {
            created = true;
            current = SceneManager.GetActiveScene().buildIndex;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Next()
    {
        current++;
        if (current > SceneManager.sceneCountInBuildSettings - 1)
        {
            current = 0;
        }

        SceneManager.LoadScene(current);
    }
}
