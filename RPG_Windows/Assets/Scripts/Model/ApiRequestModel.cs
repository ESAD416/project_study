using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ApiRequestModel
{
    private string apiUrl = string.Empty;

    public ApiRequestModel(string url) {
        apiUrl = url;
    }

    public ApiRequestModel() {
        initialize();
    }

    public void initialize() {
        apiUrl = string.Empty;
    }

    IEnumerator GetUrl(string url) {
        using (UnityWebRequest getRequest = UnityWebRequest.Get(url)) {
            yield return getRequest.SendWebRequest();
            if (getRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(getRequest.error);
            }
   
            byte[] result = getRequest.downloadHandler.data;
            // TODO
        }
    }

    IEnumerator PostUrl(string url, WWWForm form) {
        using (UnityWebRequest postRequest = UnityWebRequest.Post(url, form)) {
            yield return postRequest.SendWebRequest();
            if (postRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(postRequest.error);
            }
   
            byte[] result = postRequest.downloadHandler.data;
            // TODO
        }
    }
}
