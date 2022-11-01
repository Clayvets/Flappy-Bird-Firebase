using System.Collections;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;

public class Request : MonoBehaviour
{
    [SerializeField]Button mybutton;
     string UserId;
    DatabaseReference mDatabase;
    [SerializeField] GameObject parent;


    void Start()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        mybutton.onClick.AddListener(SendRequest);
    }
    private void OnEnable() {
        
    }

    public void SendRequest() {
        //UserData user = new UserData();
        //user.sendRequest.Add(parent.name);
       
        //string json = JsonUtility.ToJson(user);
        var requests = FirebaseDatabase.DefaultInstance.GetReference("users").Child(UserId).Child("sendRequest");
        //FirebaseDatabase.DefaultInstance.GetReference("users").Child(UserId).Child("sendRequest");
        mDatabase.Child("users").Child(UserId).Child("sendRequest").SetValueAsync(parent.name);
        //mDatabase.Child("users").Child(UserId).Child("requestReceived").SetValueAsync();
        
        print("MANDE TODO");
    }
}
