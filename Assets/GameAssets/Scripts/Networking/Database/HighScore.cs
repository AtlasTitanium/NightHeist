using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HighScore : MonoBehaviour
{
    public Text hiderHighScore;
    public Text seekerHighScore;

    private string DatabaseID;
    void Start(){
        DatabaseID = CurrentDatabaseID.Instance.id;
        StartCoroutine(GetHighScore_Hider());
        StartCoroutine(GetHighScore_Seeker());
    }

    //Get and Set High score (Hider)
    IEnumerator ServerGetHighScore_Hider(){
        UnityWebRequest www = UnityWebRequest.Get("https://studenthome.hku.nl/~pepijn.kok/PHPstuff/GetHighScore_Hider.php");
        yield return www.SendWebRequest();

        //check for errors
        if(www.downloadHandler.text.Contains("ERROR")){
            Debug.Log("Can't set score");
        }
        if(www.downloadHandler.text.Contains("Failed")){
            Debug.Log("School servers out");
        }

        Scores myObject = JsonUtility.FromJson<Scores>(www.downloadHandler.text);
        hiderHighScore.text = myObject.username + " has " + myObject.AmountOfWins + " wins";
    }

    IEnumerator GetHighScore_Hider(){
        UnityWebRequest www = UnityWebRequest.Get("http://" + DatabaseID + "/PHPstuff/GetHighScore_Hider.php");
        yield return www.SendWebRequest();

        //check for errors
        if(www.downloadHandler.text.Contains("ERROR")){
            Debug.Log("Can't set score");
        }
        if(www.downloadHandler.text.Contains("Failed")){
            StartCoroutine(ServerGetHighScore_Hider());
        }

        Scores myObject = JsonUtility.FromJson<Scores>(www.downloadHandler.text);
        hiderHighScore.text = myObject.username + " has " + myObject.AmountOfWins + " wins";
    }


    //Get and Set High score (Seeker)
    IEnumerator ServerGetHighScore_Seeker(){
        UnityWebRequest www = UnityWebRequest.Get("https://studenthome.hku.nl/~pepijn.kok/PHPstuff/GetHighScore_Seeker.php");
        yield return www.SendWebRequest();

        //check for errors
        if(www.downloadHandler.text.Contains("ERROR")){
            Debug.Log("Can't set score");
        }
        if(www.downloadHandler.text.Contains("Failed")){
            Debug.Log("School servers out");
        }

        Scores myObject = JsonUtility.FromJson<Scores>(www.downloadHandler.text);
        seekerHighScore.text = myObject.username + " has " + myObject.AmountOfWins + " wins";
    }

    IEnumerator GetHighScore_Seeker(){
        UnityWebRequest www = UnityWebRequest.Get("http://" + DatabaseID + "/PHPstuff/GetHighScore_Seeker.php");
        yield return www.SendWebRequest();

        //check for errors
        if(www.downloadHandler.text.Contains("ERROR")){
            Debug.Log("Can't set score");
        }
        if(www.downloadHandler.text.Contains("Failed")){
            StartCoroutine(ServerGetHighScore_Seeker());
        }

        Scores myObject = JsonUtility.FromJson<Scores>(www.downloadHandler.text);
        seekerHighScore.text = myObject.username + " has " + myObject.AmountOfWins + " wins";
    }
}

public class Scores {
    public string username;
    public string AmountOfWins;
}
