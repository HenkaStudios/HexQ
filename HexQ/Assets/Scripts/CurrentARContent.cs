using UnityEngine;

public class CurrentARContent : MonoBehaviour
{
    private static CurrentARContent _instance;

    public static CurrentARContent Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CurrentARContent>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "Singleton Instance of " + typeof(CurrentARContent).Name;
                    _instance = go.AddComponent<CurrentARContent>();
                    DontDestroyOnLoad(_instance);
                }
            }
            return _instance;
        }
    }

    public GameObject ARPrefab;

    public void SetARPrefab(GameObject _arPrefab)
    {
        ARPrefab = _arPrefab;
    }
}
