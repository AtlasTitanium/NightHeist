using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetDatabaseID : MonoBehaviour
{
    public InputField idField;
    private bool works = true;
    public void SetID(){
        string[] c = idField.text.Split('.');
        if(c.Length == 4){
            foreach(string s in c){
                if(IsDigitsOnly(s)){
                    Debug.Log("only has digits");
                    continue;
                } else {
                    Debug.Log("Error: it contains letters");
                    works = false;
                }
            }
        } else {
            Debug.Log("wrong amount of points");
            works = false;
        }

        if(works){
            CurrentDatabaseID.Instance.id = idField.text;
        }
    }

    

    public bool IsDigitsOnly(string str) {
        foreach (char c in str)
        {
            if (c < '0' || c > '9')
                return false;
        }

        return true;
    }
}
