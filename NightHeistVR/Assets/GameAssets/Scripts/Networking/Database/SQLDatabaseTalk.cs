using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SQLDatabaseTalk : MonoBehaviour
{ 
    public GameObject main;
    public InputField username;
    public InputField pass;
    public Button button;
    private string loginName;
    private string loginPassword;
    public bool register = false;
    public string gameSelectSceneName;

    private string DatabaseID;
    void Start(){
        DatabaseID = CurrentDatabaseID.Instance.id;
        button.onClick.AddListener(Connector);
    }

    void Connector(){
        StartCoroutine(LocalConnect());
    }
    IEnumerator ServerConnect(){
        loginName = username.text;
        loginPassword = pass.text;

        WWWForm form = new WWWForm();
        form.AddField("LoginName", loginName);
        form.AddField("LoginPassword", loginPassword);

        if(register){
            using(UnityWebRequest www = UnityWebRequest.Post("https://studenthome.hku.nl/~pepijn.kok/PHPstuff/Register.php", form)){
                yield return www.SendWebRequest();

                Debug.Log(www.downloadHandler.text);
                if(www.isNetworkError){
                    Debug.Log("NetworkError");
                } else
                if(www.downloadHandler.text.Contains("ERROR")){
                    Debug.Log("NetworkError");
                } else {
                    main.SetActive(true);
                    this.gameObject.SetActive(false);
                }
            }
        } else {
            using(UnityWebRequest www = UnityWebRequest.Post("https://studenthome.hku.nl/~pepijn.kok/PHPstuff/Login.php", form)){
                yield return www.SendWebRequest();

                Debug.Log(www.downloadHandler.text);
                if(www.isNetworkError){
                    Debug.Log("NetworkError");
                } else
                if(www.downloadHandler.text.Contains("ERROR")){
                    Debug.Log("NetworkError");
                } else {
                    SceneManager.LoadScene(gameSelectSceneName);
                }
            }
        }
    }

    IEnumerator LocalConnect(){
        loginName = username.text;
        loginPassword = pass.text;

        WWWForm form = new WWWForm();
        form.AddField("LoginName", loginName);
        form.AddField("LoginPassword", loginPassword);

        if(register){Debug.Log(DatabaseID);
            using(UnityWebRequest www = UnityWebRequest.Post("http://" + DatabaseID + "/PHPstuff/Register.php", form)){
                yield return www.SendWebRequest();
                Debug.Log(www.downloadHandler.text);
                if(www.isNetworkError){
                    StartCoroutine(ServerConnect());
                } else
                if(www.downloadHandler.text.Contains("ERROR")){
                    StartCoroutine(ServerConnect());
                } else {
                    main.SetActive(true);
                    this.gameObject.SetActive(false);
                }
            }
        } else {
            using(UnityWebRequest www = UnityWebRequest.Post("http://" + DatabaseID + "/PHPstuff/Login.php", form)){
                yield return www.SendWebRequest();

                Debug.Log(www.downloadHandler.text);
                if(www.isNetworkError){
                    StartCoroutine(ServerConnect());
                } else
                if(www.downloadHandler.text.Contains("ERROR")){
                    StartCoroutine(ServerConnect());
                } else {
                    SceneManager.LoadScene(gameSelectSceneName);
                }
            }
        }
    }
}
