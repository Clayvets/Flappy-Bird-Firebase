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
    string UserId;
    public string nameUser;
    DatabaseReference mDatabase;
    [SerializeField] GameObject parent;
    GameState _GameState;
    [SerializeField] Friends friends;
    [SerializeField] GameObject amigoConectado;
    GameObject canvaAmigo;


    void Start() {
        _GameState = GameObject.Find("Controller2").GetComponent<GameState>();
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        //mDatabase.Child("users").ChildAdded += SendRequest;
        mybutton.onClick.AddListener(startRequestController);
        canvaAmigo= GameObject.Find("ListaAmigos");
    }

    private void SendRequest() {
        mDatabase.Child("users").Child(id).Child("request").Child(UserId).Child("user").SetValueAsync(_GameState.Username);
        mDatabase.Child("users").Child(id).Child("request").ChildRemoved += HandleChildRemoved;
       //mDatabase.Child("users").Child(id).Child("request").Child("user").SetValueAsync(_GameState.Username);

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
    private void HandleChildRemoved(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        GameObject desAmigo = GameObject.Find(nameUser);
        Destroy(desAmigo);
        GameObject amigo = Instantiate(amigoConectado, canvaAmigo.transform);
        amigo.name = nameUser;
        amigo.GetComponentInChildren<Text>().text = nameUser;
        //Dictionary<string, object> userDisconnected = (Dictionary<string, object>)args.Snapshot.Value;


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
