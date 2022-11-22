using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    int current;
    public static bool created;
    public Transform transitionTransform;
    Animator transitionAnim;
    bool waiting;

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
            transitionAnim = GetComponent<Animator>();
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        transitionTransform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
    }

    public void Next()
    {
        if (!waiting)
        {
            current++;
            if (current > SceneManager.sceneCountInBuildSettings - 1)
            {
                current = 0;
            }
            StartCoroutine(WaitForNext());
        }

        
    }

    public void OpenLevel(int levelIndex)
    {
        current = levelIndex;
        SceneManager.LoadScene(current);
    }

    IEnumerator WaitForNext()
    {
        waiting = true;
        transitionAnim.SetTrigger("Next");
        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene(current);
        waiting = false;
    }


    public void Die() 
    {
        if (!waiting)
        {
            StartCoroutine(WaitToRestart());
        }
    }
    IEnumerator WaitToRestart()
    {
        waiting = true;
        yield return new WaitForSeconds(0.83f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        waiting = false;
    }


}
