using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.UI;
using System;

public class MatchManager : MonoBehaviour {

    DatabaseReference mDatabase;
    GameState _GameState;
    string UserId;
    Text text;
    [SerializeField] int  users =0, usersNeeded = 2;
    [SerializeField] Button binitMatch, bexitMatch;
    [SerializeField] GameObject canvaMatch;
    // Start is called before the first frame update
    void Start()
    {

        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        _GameState = GameObject.Find("Controller2").GetComponent<GameState>();
        UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var userMatchRef = FirebaseDatabase.DefaultInstance.GetReference("match");
        mDatabase.Child("match").ChildAdded += HandleChildAdded;
        mDatabase.Child("match").ChildRemoved += HandleChildRemoved;
        binitMatch.onClick.AddListener(initMatch);
        bexitMatch.onClick.AddListener(exitMatch);
    }

    private void exitMatch() {
        mDatabase.Child("match").Child(UserId).SetValueAsync(null);
        canvaMatch.SetActive(false);
        
    }

    void initMatch() {
        
        mDatabase.Child("match").Child(UserId).Child("username").SetValueAsync(_GameState.Username);
        canvaMatch.SetActive(true);
    }

    private void HandleChildAdded(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        Dictionary<string, object> matchUsers = (Dictionary<string, object>)args.Snapshot.Value;
        users+= 1;
        text.text=canvaMatch.GetComponentInChildren<Text>().text = users + "/" + usersNeeded;
        Debug.Log("Numero "+ users +"Estan en sala: "+ matchUsers["username"]);
        if (users >= usersNeeded) text.text = "Ready"; 
       
        

    }
    private void HandleChildRemoved(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        //Dictionary<string, object> userDisconnected = (Dictionary<string, object>)args.Snapshot.Value;
       

    }

}
