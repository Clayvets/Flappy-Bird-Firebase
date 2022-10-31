using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using System;
using Firebase.Database;
using Firebase.Extensions;

public class UsernameLabel : MonoBehaviour
{
    [SerializeField] InputField email;

    [SerializeField] private Text _label;
    public static string usernameStatic;
    private void Reset()
    {
        _label = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthChange;
    }

    private void HandleAuthChange(object sender, EventArgs e)
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        string username;
        
        if (currentUser != null)
        {
            username = currentUser.UserId;
            SetLabelUsername(username);
            
            //string name = currentUser.DisplayName;
            //string email = currentUser.Email;
            //Debug.Log("Email:" + email);
        }

    }

    private void SetLabelUsername(string UserId)
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("users/" + UserId + "/username")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                    _label.text = "NULL";

                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    Debug.Log(snapshot.Value);

                    _label.text = (string)snapshot.Value;
                    usernameStatic = (string)snapshot.Value;

                }
            });

    }
    
    public void ResetPasword()
    {
        string emailAddress = "user@example.com";
        emailAddress = email.text;
        if (email.text != null) {
            FirebaseAuth.DefaultInstance.SendPasswordResetEmailAsync(emailAddress).ContinueWith(task => {
                if (task.IsCanceled) {
                    Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                    return;
                }
                if (task.IsFaulted) {
                    Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                    return;
                }
                Debug.Log("Password reset email sent successfully.");
            });
        }
    }



}