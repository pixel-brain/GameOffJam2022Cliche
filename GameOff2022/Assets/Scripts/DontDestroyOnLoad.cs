using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static bool created;
    // Start is called before the first frame update
    void Awake()
    {
        if (created)
        {
            Destroy(gameObject);
        }
        else
        {
            created = true;
            DontDestroyOnLoad(gameObject);
        }
    }


}
