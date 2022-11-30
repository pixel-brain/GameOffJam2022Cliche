using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static bool created;
    private FMOD.Studio.EventInstance BGM;
    private int scene;
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
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Update()
    
    {
       if (SceneManager.GetActiveScene().buildIndex == 0) {
            BGM.setParameterByName("Intensity", 0);
        } else if (SceneManager.GetActiveScene().buildIndex >= 1 && SceneManager.GetActiveScene().buildIndex < 3)
        {
            BGM.setParameterByName("Intensity", 1);
        }
        else if (SceneManager.GetActiveScene().buildIndex >= 3 && SceneManager.GetActiveScene().buildIndex < 6)
        {
            BGM.setParameterByName("Intensity", 2);
        }
        else if (SceneManager.GetActiveScene().buildIndex >= 6 && SceneManager.GetActiveScene().buildIndex < 11)
        {
            BGM.setParameterByName("Intensity", 3);
        }
        else if (SceneManager.GetActiveScene().buildIndex >= 11 && SceneManager.GetActiveScene().buildIndex < 15)
        {
            BGM.setParameterByName("Intensity", 4);
        }
        else if (SceneManager.GetActiveScene().buildIndex >= 15 && SceneManager.GetActiveScene().buildIndex <= 17)
        {
            BGM.setParameterByName("Intensity", 5);
        }
    }
    private void OnDestroy()
    {
        BGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        BGM.release();
       


    }
 


}
