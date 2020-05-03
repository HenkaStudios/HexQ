//using System.Collections.Generic;
//using UnityEngine;

//public class MicrobeModelsController : APIDataManager
//{
//    public static MicrobeModelsController instance;

//    public List<GameObject> microbeModels = new List<GameObject>();

//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else if (instance != this)
//        {
//            Destroy(gameObject);
//        }
//    }

//    public void DownloadAssetBundles()
//    {
//        var tempListOfMicrobeNames = new List<string>();
//        for (int i = 0; i < ApiData.microbeAssociatedInfo.microbe.microbeData.Count; i++)
//        {
//            var currentMicrobeName = ApiData.microbeAssociatedInfo.microbe.microbeData[i].microbeName;
//            tempListOfMicrobeNames.Add(currentMicrobeName);
//        }
//        ApiData.SetUrlAndGetAssetBundles(tempListOfMicrobeNames, GetFetchedModelsAndPopulateList);
//    }

//    void GetFetchedModelsAndPopulateList(List<GameObject> _fetchedModelList)
//    {
//        microbeModels = _fetchedModelList;
//        AssignModelRespectedToGlass();
//    }

//    void AssignModelRespectedToGlass()
//    {
//        for (int i = 0; i < microbeModels.Count; i++)
//        {
//            MicrobeController.glassBtns[i].GetComponent<GlassBtn>().microbeModel = microbeModels[i];
//        }
//    }

//}
