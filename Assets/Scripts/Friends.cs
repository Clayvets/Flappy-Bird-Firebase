using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;

public class Friends : MonoBehaviour {
    string UserId;
    string nameOther;
    GameState _GameState;
    [SerializeField] Text mens;
    [SerializeField] GameObject panel;
    [SerializeField] Button  acceptButton;
    DatabaseReference mDatabase;
    private void Start() {
        UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        _GameState = GameObject.Find("Controller2").GetComponent<GameState>();
        print("ESTOY EN FRIENDS");
        _GameState.OnDataReady += InitUsersOnlineController;
       
       // mDatabase.Child("request").ChildAdded += HandleChildAddedRequest;

    }
    public void InitUsersOnlineController() {
        print("la envié");
        FirebaseDatabase.DefaultInstance.LogLevel = LogLevel.Verbose;
        var request = FirebaseDatabase.DefaultInstance.GetReference("users").Child(UserId).Child("request");
        mDatabase.Child("request").ChildAdded += HandleChildAddedRequest;
    }
    private void HandleChildAddedRequest(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        print("ESTOY PIENDO SER AMIGO");
        Dictionary<string, object> userAdded = (Dictionary<string, object>)args.Snapshot.Value;
        nameOther = (string)userAdded["username"];
        acceptButton = GameObject.Find("Accept").GetComponent<Button>();
        panel.SetActive(true);
        mens.text = nameOther + "  wants to be your friend!";
        acceptButton.onClick.AddListener(AcceptFriend);

        // mDatabase.Child("users").Child(UserId).Child("request").SetValueAsync((string)userAddedToRequest["username"]);
        //Button addButton = newAddButton.GetComponent<Button>();
        //addButton.onClick.AddListener(AcceptRequest);

    }
    private void AcceptFriend() {
        
       
        mDatabase.Child("users").Child(UserId).Child("friends").SetValueAsync(nameOther);
    }
}

