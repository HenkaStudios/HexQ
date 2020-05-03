using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class APIDataManager : MonoBehaviour
{
    public APIData ApiData { get { return FindObjectOfType<APIData>(); } }
}

public class APIData : MonoBehaviour
{
    public MicrobeAssociatedInfo microbeAssociatedInfo;

    private string urlDomain = "https://hexq2.000webhostapp.com";

    // Start is called before the first frame update
    void Start()
    {
        SetUrlAndFetchMicrobeDetails();
    }

    void SetUrlAndFetchMicrobeDetails()
    {
        var url = urlDomain + "/Microbe.json";
        StartCoroutine(RestServices.Instance.Get(url,
           GetResponseAndPopulateMicrobeClass,
           microbeAssociatedInfo.functionsToBeInvokedAfterResponse));
    }

    void GetResponseAndPopulateMicrobeClass(string _string)
    {
        microbeAssociatedInfo.microbe = MicrobeWrapper.CreateFromJSON(_string);
    }


    public void SetUrlAndGetAssetBundles(List<string> _objectNames, Action<List<GameObject>> _callback)
    {
        var url = urlDomain + "/3DModels/";
        StartCoroutine(RestServices.Instance.GetAssetBundles(url,
            _objectNames,
            _callback));
    }

    public void SetUrlAndGetImages(List<string> _objectNames, Action<List<Texture>> _callback)
    {
        var url = urlDomain + "/MicrobeImages/";
        StartCoroutine(RestServices.Instance.GetTextures(url,
            _objectNames,
            _callback,
            null));
    }
}

[System.Serializable]
public class MicrobeAssociatedInfo
{
    public MicrobeWrapper microbe;
    public UnityEvent functionsToBeInvokedAfterResponse;
}
