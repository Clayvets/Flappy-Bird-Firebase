using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonLogout : MonoBehaviour, IPointerClickHandler
{
    private DatabaseReference mDatabase;

    private string UserId;

    public event Action OnLogout;
    // Start is called before the first frame update
    void Start()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        FirebaseAuth.DefaultInstance.SignOut();
        print("Hola mami salí");
        mDatabase.Child("users-online").Child(UserId).SetValueAsync(null);
        SceneManager.LoadScene(1);
        
        
    }
  
}