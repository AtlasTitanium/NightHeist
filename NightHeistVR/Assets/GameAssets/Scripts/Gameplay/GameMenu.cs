using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameMenu : MonoBehaviour
{
    [Header("UI elements")]
    public Button hostGame;
    public Button joinGame;
    public InputField serverName;
              
    private string DatabaseID;
    void Start(){
        DatabaseID = CurrentDatabaseID.Instance.id;
        hostGame.onClick.AddListener(StartHost);
        joinGame.onClick.AddListener(StartJoin);
    }

    void StartHost(){
        if(serverName.text != ""){
            StartCoroutine(CreateLocalHost());
        }
        
    }

    void StartJoin(){
        if(serverName.text != ""){
            StartCoroutine(CreateLocalClient());
        }
    }

    #region Server connect
    //Create server locally with xampp
    IEnumerator CreateLocalHost(){
        WWWForm form = new WWWForm();
        form.AddField("name", serverName.text);
        form.AddField("ip", IPManager.GetLocalIPAddress());
        UnityWebRequest www = UnityWebRequest.Post("http://" + DatabaseID + "/PHPstuff/CreateServer.php", form);
        yield return www.SendWebRequest();
        if(www.downloadHandler.text == "0"){
            GameData.Instance.StartServer();
        }
        
        //check for connect error
        if(www.downloadHandler.text.Contains("Failed")){
            Debug.Log("Server out");
        }
    }

    #endregion

    #region Client connect
    //join client locally with xampp
    IEnumerator CreateLocalClient(){
        WWWForm form = new WWWForm();
        form.AddField("name", serverName.text);
        UnityWebRequest www = UnityWebRequest.Post("http://" + DatabaseID + "/PHPstuff/JoinServer.php", form);
        yield return www.SendWebRequest();
        string[] ipAdressNumbers = www.downloadHandler.text.Split('.');
        if(ipAdressNumbers.Length == 4){
            GameData.Instance.StartClient(www.downloadHandler.text);
        } else {
            Debug.LogError("Error: ip is not correct - your given ip adress either contains not enough numbers, or too many");
        }

        //check for connect error
        if(www.downloadHandler.text.Contains("Failed")){
            Debug.Log("Server out");
        }
    }

    #endregion
}
