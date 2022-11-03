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
    [SerializeField] GameObject usuarioConectado, amigoConectado, notificacion;
    [SerializeField] GameObject canvasPadre, canvaAmigo;

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
       
        mDatabase.Child("users-online").ChildAdded += HandleChildAdded;
        mDatabase.Child("users-online").ChildRemoved += HandleChildRemoved;
        //print("ESTOY ONLINE BABEEE");
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
        //Debug.Log("Mis ARGS: " + args.Snapshot.Value);
        if (_GameState.Username == (string)userConnected["username"]) { return;}
        if (_GameState.friends.ContainsKey(args.Snapshot.Key)) {
            print("Tengo un amigo: " + (string)userConnected["username"]);
            GameObject amigo = Instantiate(amigoConectado, canvaAmigo.transform);
            amigo.name = (string)userConnected["username"];
            AmigoEstadoNotificacion(" has connected", (string)userConnected["username"]);
            amigo.GetComponentInChildren<Text>().text = (string)userConnected["username"];
            return;
        }
        //print("Mi amigo: " + (string)userConnected);
        GameObject usuario = Instantiate(usuarioConectado, canvasPadre.transform);
        usuario.name = (string)userConnected["username"];
        usuario.GetComponentInChildren<Button>().gameObject.GetComponent<Request>().id = args.Snapshot.Key;
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
        if (_GameState.friends.ContainsKey(args.Snapshot.Key)) {
            AmigoEstadoNotificacion(" has disconnected", (string)userDisconnected["username"]);
        }
       
        
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
    void AmigoEstadoNotificacion(string m, string name) {
        notificacion.SetActive(true);
        notificacion.GetComponentInChildren<Text>().text =name + m;
        Invoke("DesactivarNotificacion", 2);
    }

    void DesactivarNotificacion() {
        notificacion.SetActive(false);
    }

    
}
    

