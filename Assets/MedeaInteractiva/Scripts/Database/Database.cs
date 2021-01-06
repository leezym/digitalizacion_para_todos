using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Database : MonoBehaviour
{
    //Example: https://servidor.com/myphp.php?
    [Tooltip("Example: https://servidor.com/myphp.php?")]
    [TextArea]
    public string[] url;
    public string secretKey;
    public string userId;
    public string typeUser;

    protected bool connecting;
    public bool onConnection;
    public bool isTest;
    public bool connectionError;

    public UnityWebRequest WebRequestGET(string url, string parameters)
    {
        if (url.Length == 0 || string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(userId))
        {
            Debug.LogError("Por favor complete los campos de la base de datos.");
            return null;
        }

        string uri = string.Format("{0}{1}", url, parameters);

        UnityWebRequest www = UnityWebRequest.Get(uri);

        www.SetRequestHeader("Accept", "*/*");

#if !UNITY_WEBGL
        www.SetRequestHeader("User-Agent", "runscope/0.1");
#elif UNITY_EDITOR
        www.SetRequestHeader("User-Agent", "runscope/0.1");
#endif
        return www;
    }

    public UnityWebRequest WebRequestPOST(string url, WWWForm parameters)
    {
        if (url.Length == 0 /*|| string.IsNullOrEmpty(secretKey)*/ || string.IsNullOrEmpty(userId))
        {
            Debug.LogError("Por favor complete los campos de la base de datos.");
            return null;
        }

        string uri = string.Format("{0}{1}", url, parameters);

        UnityWebRequest www = UnityWebRequest.Post(url, parameters);

        www.SetRequestHeader("Accept", "*/*");

#if !UNITY_WEBGL
        www.SetRequestHeader("User-Agent", "runscope/0.1");
#elif UNITY_EDITOR
        www.SetRequestHeader("User-Agent", "runscope/0.1");
#endif
        return www;
    }

    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
        return hashString.PadLeft(32, '0');
    }
}
