using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static bool created;
    private FMOD.Studio.EventInstance BGM;
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
    private void OnDestroy()
    {
        BGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        BGM.release();
    }


}
