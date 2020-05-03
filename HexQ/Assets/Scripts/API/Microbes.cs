using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Microbe
{
    public string microbeName;
    public int glassID;
    public string TimeStamp;
    public string Kingdom;
    public string Phylum;
    public string Class;
    public string Order;
    public string Family;
    public string Genus;
    public string Species;
    public string description;
    public string Report;
    
}

[System.Serializable]
public class MicrobeWrapper
{
    public List<Microbe> microbeData;

    public static MicrobeWrapper CreateFromJSON(string _json)
    {
        return JsonUtility.FromJson<MicrobeWrapper>(_json);
    }

    public static string ConvertToJSON(MicrobeWrapper _microbeWrapper)
    {
        return JsonUtility.ToJson(_microbeWrapper);
    }
}