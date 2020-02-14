using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerManager : MonoBehaviour
{
    public static ServerManager Instance { get; private set; }

    //Client and Server Prefabs
    public GameObject client;
    public GameObject server;

    //Other
    private GameData gameData;
    [HideInInspector]
    public Server currentServer;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }

    void Start(){
        gameData = GameData.Instance;
        if(gameData == null){
            Debug.LogWarning("Server Manager cannot find GameData script. \nStart game from MainMenu Scene.");
            return;
        }
        
        //Instantiate server
        if(gameData.isServer){
            gameData.ipString = IPManager.GetLocalIPAddress();
            
            currentServer = Instantiate(server,transform.position,Quaternion.identity).GetComponent<Server>();
        }
    }
}
