using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameData : MonoBehaviour
{
    //Singleton
    private static GameData _instance;

    public static GameData Instance { get { return _instance; } }


    //Variables
    public string gameSceneName;
    
    [HideInInspector]
    public string ipString;
    [HideInInspector]
    public bool isClient;
    [HideInInspector]
    public bool isServer;
    private string DatabaseID;

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

    void Start(){
        DatabaseID = CurrentDatabaseID.Instance.id;
    }
    
    public void StartServer(){
        isServer = true;
        isClient = true;
        SceneManager.LoadScene(gameSceneName);
    }

    public void StartClient(string ip){
        ipString = ip;
        isClient = true;
        SceneManager.LoadScene(gameSceneName);
    }

    IEnumerator LocalRemoveServer(){
        WWWForm form = new WWWForm();
        form.AddField("ip", IPManager.GetLocalIPAddress());
        UnityWebRequest www = UnityWebRequest.Post("http://" + DatabaseID + "/PHPstuff/RemoveServer.php", form);
        yield return www.SendWebRequest();
        if(www.downloadHandler.text == "0"){
            Debug.Log("Server removed");
        }
    }
}

public static class IPManager {
    public static string GetLocalIPAddress() {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList) {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                return ip.ToString();
            }
        }

        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
}


