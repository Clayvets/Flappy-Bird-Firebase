using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Linq;

public class FB_ScoreController : MonoBehaviour
{

    DatabaseReference mDatabase;
    string UserId;
    private List<UserInfo> maxScoresData = new List<UserInfo>();
    public Dictionary<string, object> userObject;
    [SerializeField]private Text laderNames;
    [SerializeField]private Text laderScores;
    
    public int maxLocalScore;
    // Start is called before the first frame update
    void Start()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
    }

    public void WriteNewScore(int score)
    {
        UserData data = new UserData();

        data.score = score;
        data.username = UsernameLabel.usernameStatic;
        string json = JsonUtility.ToJson(data);

        mDatabase.Child("users").Child(UserId).SetRawJsonValueAsync(json);
        
    }

    public void GetUserScore()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("users/"+UserId)
            .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    Debug.Log(snapshot.Value);

                    var data = (Dictionary<string, object>)snapshot.Value;

                    Debug.Log("Puntaje: " +data["score"]);
                    
                    

                    //foreach (var userDoc in (Dictionary<string,object>)snapshot.Value)
                    //{
                    //    Debug.Log(userDoc.Key);
                    //    Debug.Log(userDoc.Value);

                    //}
                    // Do something with snapshot...
                }
            });
    }

    public void GetUsersHighestScore()
    {
        /*FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("score").LimitToLast(5)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    //Handle the error...
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log(snapshot);

                    foreach (var userDoc in (Dictionary<string,object>)snapshot.Value)
                    {

                        var userObject = (Dictionary<string, object>)userDoc.Value;
                        Debug.Log(userObject["username"] + ":" + userObject["score"]);

                    }
                    
                }
            });*/
        FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("score").LimitToLast(5).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCompleted)
            {
                List<UserInfo> maxScoresData2 = new List<UserInfo>();
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot);
  

                foreach (var userDoc in (Dictionary<string, object>)snapshot.Value)
                {

                    userObject = (Dictionary<string, object>)userDoc.Value;

                    Debug.Log((userObject["username"])+":"+userObject["score"]);
                    UserInfo userInfo2 = new UserInfo();
                    userInfo2.score = userObject["score"].ToString();
                    userInfo2.username = userObject["username"].ToString();
                    maxScoresData2.Add(userInfo2);
                    Debug.Log("Lista Creada con exito");
                }
                
                Debug.Log("Iniciando lista");       
                
                laderNames.text = "Names: \n";
                laderScores.text = "Scores: \n";
                foreach (var VARIABLE in maxScoresData2.OrderByDescending(maxScoresData2 => maxScoresData2.score))
                {
                    laderNames.text += VARIABLE.username + "\n";
                    laderScores.text += VARIABLE.score + "\n";
                }
                
            }
        });
    }
    
}

public class UserData
{
    public int score;
    public string username;
    public string[] friends;
    public string[] request;
    public string[] sendRequest;
}

public class UserInfo
{
    public string score;
    public string username;
}