using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;

public class UserOnlineController : MonoBehaviour
{
    // Start is called before the first frame update
    DatabaseReference mDatabase;
    GameState _GameState;
    string UserId;
    [SerializeField]
    ButtonLogout _ButtonLogout;
    [SerializeField] GameObject usuarioConectado;
    [SerializeField] GameObject canvasPadre;

    void Start()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        _GameState = GameObject.Find("Controller2").GetComponent<GameState>();
       
        _GameState.OnDataReady += InitUsersOnlineController;
        UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

    }

    public void InitUsersOnlineController()
    {
        FirebaseDatabase.DefaultInstance.LogLevel = LogLevel.Verbose;
        //Debug.Log("Init users online controller");
        //_ButtonLogout.OnLogout += SetUserOffline;
        var userOnlineRef = FirebaseDatabase.DefaultInstance.GetReference("users-online");
       // print("ESTOY ONLINE BABEEE");
        mDatabase.Child("users-online").ChildAdded += HandleChildAdded;
        mDatabase.Child("users-online").ChildRemoved += HandleChildRemoved;

        SetUserOnline();
    }
    private void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {  
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        Dictionary<string, object> userConnected = (Dictionary<string, object>)args.Snapshot.Value;
        Debug.Log(userConnected["username"] + " is online");
        if (_GameState.Username == (string)userConnected["username"]) {return;}
        GameObject usuario = Instantiate(usuarioConectado, canvasPadre.transform);
        usuario.name = (string)userConnected["username"];
        usuario.GetComponentInChildren<Text>().text = (string)userConnected["username"];
        
    }
    private void HandleChildRemoved(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Dictionary<string, object> userDisconnected = (Dictionary<string, object>)args.Snapshot.Value;
        GameObject desconectado = GameObject.Find((string)userDisconnected["username"]);
        Destroy(desconectado);
        Debug.Log(userDisconnected["username"] + " is offline");
    }


    private void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if(args.DatabaseError!= null)
        {
            Debug.Log(args.DatabaseError.Message);
            return;
        }
        Dictionary<string,object> usersList = (Dictionary<string, object>)args.Snapshot.Value;
        
        if(usersList != null)
        {
            foreach (var userDoc in usersList)
            {
                Dictionary<string, object> userOnline = (Dictionary<string, object>)userDoc.Value;
                Debug.Log("ONLINE:" + userOnline["username"]);
                
            }
        }
        
    }
    private void SetUserOnline()
    {
        mDatabase.Child("users-online").Child(UserId).Child("username").SetValueAsync(_GameState.Username);
    }
    //private void SetUserOffline()
    //{  
    //    mDatabase.Child("users-online").Child(UserId).SetValueAsync(null);
    //}

    //void OnApplicationQuit()
    //{
    //    SetUserOffline();
    //}

    
}
    

