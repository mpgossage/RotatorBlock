﻿using UnityEngine;
using System.Collections;

// http://www.kongregate.com/developer_center/docs/en/using-the-api-with-unity3d
// read http://www.kongregate.com/developer_center/docs/en/statistics-api-tips for very good advice
// http://developers.kongregate.com/blog/unity-webgl
public class Kongregate: MonoBehaviour 
{
    bool isKongregateReady;
    int userId;
    string username;
    //string gameAuthToken;

    public bool IsKongregateReady { get { return isKongregateReady; } }
    public int UserId { get { return userId; } }
    public string Username { get { return username; } }

    void Start()
    {
#if UNITY_WEBGL
        Debug.Log("Connecting To Kongregate");
        isKongregateReady = false;
        // Try to connect to Kongregate.
        // The gameObject.name parameter is used so SendMessage
        // will look for the OnKongregateAPILoaded method
        // on this same MonoBehaviour
        Application.ExternalEval(
        "if(typeof(kongregateUnitySupport) != 'undefined'){" +
        " kongregateUnitySupport.initAPI('" + gameObject.name + "', 'OnKongregateAPILoaded');" +
        "};"
        );
#endif
    }
#if UNITY_WEBGL
    void OnKongregateAPILoaded(string userInfoString)
    {
        Debug.Log("OnKongregateAPILoaded(" + userInfoString + ")");
        // Here I set a static variable which I can
        // check to know if Kongregate connection is ready
        isKongregateReady = true;
        // Kongregate returns a char delimited string
        // composed of userId|username|gameAuthToken
        // Here I just store them for easier access
        string[] parms = userInfoString.Split('|');
        userId = int.Parse(parms[0]); // int
        username = parms[1]; // string
        //gameAuthToken = parms[2]; // string
        Debug.Log("Kongregate User Info: " + username + ", userId: " + userId);
    }
#endif

    // submits a statistic to Kong
    // http://www.kongregate.com/developer_center/docs/en/statistics-api
    // note: stats must be non-negative integers
    public void SubmitStatistic(string name, int value)
    {
#if UNITY_WEBGL
        if (value<0)	return;
		if (!isKongregateReady)	return;
		Debug.Log("Kongregate.SubmitStatistic("+name+", "+value+")");
        Application.ExternalCall("kongregate.stats.submit", name, value);
#endif
    }

    #region Singleton Code
    private static Kongregate instance;
    public void Awake()
    {
        if (instance != null)
            Debug.LogError("Someone created too many Singletons");
    }
    public static Kongregate Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("KongregateInterface");
                instance = obj.AddComponent<Kongregate>();
                DontDestroyOnLoad(instance.gameObject);	// don't get destroyed in a level loading
            }
            return instance;
        }
    }

    public void OnApplicationQuit()
    {
        Destroy(instance.gameObject);
        instance = null;
    }
    #endregion
}
