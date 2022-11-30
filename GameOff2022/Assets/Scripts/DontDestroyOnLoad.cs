using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static bool created;
    private FMOD.Studio.EventInstance BGM;
    private int prevSceneNum;
    // Start is called before the first frame update
    void Awake()
    {
        if (created)
        {
            Destroy(gameObject);
        }
        else
        {
            BGM = FMODUnity.RuntimeManager.CreateInstance("event:/BGM/Level");
            BGM.start();
            created = true;
            prevSceneNum = -1;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Function called on scene load
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int sceneNum = scene.buildIndex;
        // If scene changed
        if (prevSceneNum != sceneNum)
        {
            if (sceneNum == 0)
            {
                BGM.setParameterByName("Intensity", 0);
            }
            else if (sceneNum < 3)
            {
                BGM.setParameterByName("Intensity", 1);
            }
            else if (sceneNum < 6)
            {
                BGM.setParameterByName("Intensity", 2);
            }
            else if (sceneNum < 11)
            {
                BGM.setParameterByName("Intensity", 3);
            }
            else if (sceneNum < 15)
            {
                BGM.setParameterByName("Intensity", 4);
            }
            else if (sceneNum <= 17)
            {
                BGM.setParameterByName("Intensity", 5);
            }
        }
        prevSceneNum = sceneNum;
    }

    private void OnDestroy()
    {
        BGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        BGM.release();
       


    }

    // Setup for function call on scene load
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


}
