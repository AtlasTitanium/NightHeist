using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentDatabaseID : MonoBehaviour
{
    //Singleton
    private static CurrentDatabaseID _instance;

    public static CurrentDatabaseID Instance { get { return _instance; } }
    //Variable
    public string id;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
