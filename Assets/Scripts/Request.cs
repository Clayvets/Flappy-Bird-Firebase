using System.Collections;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;

public class Request : MonoBehaviour
{
    [SerializeField] Button mybutton, acceptButton;
    public string id;
   
    DatabaseReference mDatabase;
    [SerializeField] GameObject parent;
    GameState _GameState;
    

    

    void Start() {
        _GameState = GameObject.Find("Controller2").GetComponent<GameState>();
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        
        //mDatabase.Child("users").ChildAdded += SendRequest;
        mybutton.onClick.AddListener(startRequestController);
    }

    private void SendRequest() {


        //string json = JsonUtility.ToJson(user);
        //startRequestController();
        //FirebaseDatabase.DefaultInstance.GetReference("users").Child(UserId).Child("sendRequest");
        //mDatabase.Child("users").Child(id).Child("sendRequest").SetValueAsync(parent.name);
        mDatabase.Child("users").Child(id).Child("request").SetValueAsync(_GameState.Username);
        
        print("MANDE TODO");
    }
    public void startRequestController() {
       // var requestsSend = FirebaseDatabase.DefaultInstance.GetReference("users").Child(UserId).Child("sendRequest");
        var friends = FirebaseDatabase.DefaultInstance.GetReference("users").Child(id).Child("friends");
        var request = FirebaseDatabase.DefaultInstance.GetReference("users").Child(id).Child("request");
        mDatabase.Child("friends").ChildAdded += FriendAdded;
        SendRequest();
       // mDatabase.Child("request").ChildAdded += HandleChildAddedRequest;
    }

    private void FriendAdded(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Dictionary<string, object> userAdded = (Dictionary<string, object>)args.Snapshot.Value;
        
        
    }
    //private void HandleChildAddedRequest(object sender, ChildChangedEventArgs args) {
    //    if (args.DatabaseError != null) {
    //        Debug.LogError(args.DatabaseError.Message);
    //        return;
    //    }
    //    Dictionary<string, object> userAdded = (Dictionary<string, object>)args.Snapshot.Value;
    //    nameOther= (string)userAdded["username"];
    //    acceptButton = GameObject.Find("Accept").GetComponent<Button>();
    //    acceptButton.onClick.AddListener(AcceptFriend);
        
    //   // mDatabase.Child("users").Child(UserId).Child("request").SetValueAsync((string)userAddedToRequest["username"]);
    //    //Button addButton = newAddButton.GetComponent<Button>();
    //    //addButton.onClick.AddListener(AcceptRequest);

    //}

    //private void AcceptFriend() {
    //    mDatabase.Child("users").Child(id).Child("friends").SetValueAsync(nameOther);
    //}

}
