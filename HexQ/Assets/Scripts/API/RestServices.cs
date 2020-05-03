using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class RestServices : MonoBehaviour
{
    private static RestServices _instance;

    public static RestServices Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RestServices>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "Singleton Instance of " + typeof(RestServices).Name;
                    _instance = go.AddComponent<RestServices>();
                    DontDestroyOnLoad(_instance);
                }
            }
            return _instance;
        }
    }

    private bool uwrErrorCheck(UnityWebRequest uwr) => uwr.isNetworkError || uwr.isHttpError;

    public IEnumerator Get(string _url, Action<string> _callback, UnityEvent _eventsToBeInvoked)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(_url);
        yield return uwr.SendWebRequest();

        if (uwrErrorCheck(uwr))
        {
            Debug.LogError(uwr.error);
        }
        else
        {
            if (uwr.isDone)
            {
                string jsonResponse = uwr.downloadHandler.text;
                _callback(jsonResponse);
                if (!object.ReferenceEquals(_eventsToBeInvoked, null))
                {
                    _eventsToBeInvoked.Invoke();
                }
            }
        }
    }

    string AddQueryParamsToUrl(string _url, List<string> _queryParams)
    {
        _url = _url + "?";
        for (int i = 0; i < _queryParams.Count; i++)
        {
            if (i < _queryParams.Count - 1)
            {
                _url += _queryParams[i] + "&";
            }
            else
            {
                _url += _queryParams[i];
            }
        }

        return _url.Replace(" ", string.Empty);
    }

    List<string> FetchQueryParamsFromType<T>(T t)
    {
        var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        var fieldNames = t.GetType().GetFields(bindingFlags).Select(field => field.Name).ToArray();
        var fieldValues = t.GetType().GetFields(bindingFlags).Select(field => field.GetValue(t)).ToArray();

        List<string> queryParams = new List<string>();

        for (int i = 0; i < fieldNames.Length; i++)
        {
            queryParams.Add(fieldNames[i] + " = " + fieldValues[i]);

        }
        return queryParams;
    }

    public IEnumerator Get<T>(string _url, Action<string> _callback, T _queryParamType, UnityEvent _eventsToBeInvoked)
    {
        _url = AddQueryParamsToUrl(_url, FetchQueryParamsFromType<T>(_queryParamType));

        UnityWebRequest uwr = UnityWebRequest.Get(_url);
        yield return uwr.SendWebRequest();

        if (uwrErrorCheck(uwr))
        {
            Debug.LogError(uwr.error);
        }
        else
        {
            if (uwr.isDone)
            {
                string jsonResponse = uwr.downloadHandler.text;
                _callback(jsonResponse);

                if (!object.ReferenceEquals(_eventsToBeInvoked, null))
                {
                    _eventsToBeInvoked.Invoke();
                }
            }
        }
    }

    public IEnumerator GetTexture(string _url, Action<Texture2D> _callback, Action _eventsToBeInvoked)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(_url);
        yield return www.SendWebRequest();
        while (!www.isDone)
        {
            yield return null;
        }

        if (uwrErrorCheck(www))
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            if (downloadedTexture)
            {
                _callback(downloadedTexture);
            }

            if (!object.ReferenceEquals(_eventsToBeInvoked, null))
            {
                _eventsToBeInvoked();
            }
        }
    }

    public IEnumerator GetTextures(string _url, List<string> _objectNames, Action<List<Texture>> _callback, Action _eventsToBeInvoked)
    {
        List<Texture> tempTextures = new List<Texture>();

        for (int i = 0; i < _objectNames.Count; i++)
        {
            if (!string.IsNullOrEmpty(_objectNames[i]))
            {
                var url = _url + _objectNames[i] + ".jpg";
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
                yield return www.SendWebRequest();

                while (!www.isDone)
                {
                    yield return null;
                }

                if (uwrErrorCheck(www))
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Texture downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                    tempTextures.Add(downloadedTexture);
                }
            }
        }

        _callback(tempTextures);
        if (!object.ReferenceEquals(_eventsToBeInvoked, null))
        {
            _eventsToBeInvoked();
        }
    }

    public IEnumerator Post(string _url, WWWForm _dataToUpdate, Action<string> callback)
    {
        UnityWebRequest uwr = UnityWebRequest.Post(_url, _dataToUpdate);
        yield return uwr.SendWebRequest();

        if (uwrErrorCheck(uwr))
        {
            Debug.LogError(uwr.error);
        }
        else
        {
            if (uwr.isDone)
            {
                string jsonResponse = uwr.downloadHandler.text;
                callback(jsonResponse);
            }
        }
    }

    public IEnumerator Put(string _url, WWWForm _dataToReplace, Action<string> callback)
    {
        UnityWebRequest uwr = UnityWebRequest.Post(_url, _dataToReplace);
        uwr.method = "PUT";
        yield return uwr.SendWebRequest();

        if (uwrErrorCheck(uwr))
        {
            Debug.Log("Error: " + uwr.error);
        }
        else
        {
            if (uwr.isDone)
            {
                string jsonResponse = uwr.downloadHandler.text;
                callback(jsonResponse);
            }
        }

    }

    public IEnumerator GetAssetBundle(string _url, string _objectName, Action<GameObject> _callback)
    {
        UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(_url + "/" + _objectName);
        yield return uwr.SendWebRequest();

        if (uwrErrorCheck(uwr))
        {
            Debug.Log("Error: " + uwr.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
            var prefab = bundle.LoadAsset<GameObject>(_objectName);
            _callback(prefab);
        }
    }

    public IEnumerator GetAssetBundles(string _url, List<string> _objectNames, Action<List<GameObject>> _callback)
    {
        List<GameObject> tempListOfDownloadedObjects = new List<GameObject>();

        for (int i = 0; i < _objectNames.Count; i++)
        {
            if (!string.IsNullOrEmpty(_objectNames[i]))
            {
                var url = _url + _objectNames[i];
                UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(url);
                Debug.Log(url);
                yield return uwr.SendWebRequest();

                if (uwrErrorCheck(uwr))
                {
                    Debug.Log("Error: " + uwr.error);
                }
                else
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
                    var prefab = bundle.LoadAsset<GameObject>(_objectNames[i]);
                    tempListOfDownloadedObjects.Add(prefab);
                }
            }
        }

        _callback(tempListOfDownloadedObjects);
        yield break;
    }

}
