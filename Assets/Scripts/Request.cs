using System.Collections;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public class Request : MonoBehaviour
{
    public string UserId;
    DatabaseReference mDatabase;

    void Start()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
    }

    public void SendRequest() {
        var userRequests = FirebaseDatabase.DefaultInstance.GetReference("users").Child(UserId).Child("sendRequest");
    }
}
