using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public int Score;
    public string Username;
    public string UserId;

    DatabaseReference mDatabase;

    public event Action OnDataReady;


    void Start() {
        
        mDatabase =FirebaseDatabase.DefaultInstance.RootReference;
        UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        GetUserData();
    }
    
    public void GetUserData(){
        print("ENTRO A GET USER DATA");
        FirebaseDatabase.DefaultInstance.GetReference("users/" + UserId).GetValueAsync().ContinueWith(task => {
            if(task.IsFaulted)
            {
                Debug.LogWarning("Hola aqui pasando");
            }
            else if(task.IsCompleted){

                DataSnapshot snapshot = task.Result;
                Dictionary<string,object> userData = (Dictionary<string,object>)snapshot.Value;
                Debug.Log(@"user connected:" + "username:" + userData["username"] + 
                          "  score: "+ userData["score"]);
                Username=(string)userData["username"];
                //Score=int.Parse((string)userData["score"]);

                OnDataReady?.Invoke();
            }
        });
    }
}
