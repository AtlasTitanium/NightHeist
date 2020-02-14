using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuitGame {

    [RuntimeInitializeOnLoadMethod]
    static void RunOnStart()
    {
        Application.wantsToQuit += WantsToQuit;
    }

    static bool WantsToQuit()
    {
        if(GameData.Instance == null){
            return true;
        } else 
        if (GameData.Instance.isClient) {
            Debug.Log("Client still on");
            return false;
        } else if (GameData.Instance.isServer) {
            Debug.Log("Server still on");
            return false;
        } else {
            Debug.Log("you're allowed to quit");
            return true;
        }        
    }
}
