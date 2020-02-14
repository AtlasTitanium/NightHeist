using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class BeginCheckForSession : MonoBehaviour
{
    public string gameSelectScene;

    private string serverRequest;
    private string localRequest;

    private string DatabaseID;
    void Start(){
        DatabaseID = CurrentDatabaseID.Instance.id;
        
        serverRequest = "https://studenthome.hku.nl/~pepijn.kok/PHPstuff/GetUsername.php";
        localRequest = "http://" + DatabaseID + "/PHPstuff/GetUsername.php";

        StartCoroutine(CheckForConnection(localRequest));
    }

    IEnumerator CheckForConnection(string request){
        UnityWebRequest www = UnityWebRequest.Get(request);
        yield return www.SendWebRequest();
        if(www.isNetworkError){
            if(request == serverRequest){
                Debug.Log("network error");
            } else {
                StartCoroutine(CheckForConnection(serverRequest));
            }
        } else 
        if(www.downloadHandler.text.Contains("ERROR")){
            Debug.Log("no username");
            this.enabled = false;
        } else {
            SceneManager.LoadScene(gameSelectScene);
        }
    }
}
