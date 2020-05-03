using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MicrobeController : APIDataManager
{
    public GameObject glassBtnPrefabToBeSpawned;
    public Transform GlassBtnsParent;
    public TextMeshProUGUI DetailsOfTheMicrobe;
    public TextMeshProUGUI timeStamp;
    public RawImage imageInDetailsPage;
    public List<GameObject> glassBtns = new List<GameObject>();
    public List<GameObject> microbeModels = new List<GameObject>();
    public List<Texture> microbeImages = new List<Texture>();

    //Miscellaneous
    public Transform DetailsPage;
    public Button splashStartButton;

    public void SpawnBtns()
    {
        for (int i = 0; i < ApiData.microbeAssociatedInfo.microbe.microbeData.Count; i++)
        {
            int j = i;
            var currentGlass = ApiData.microbeAssociatedInfo.microbe.microbeData[i];
            GameObject GO = Instantiate(glassBtnPrefabToBeSpawned);
            GO.name = currentGlass.glassID.ToString() + " with " + currentGlass.microbeName;
            GO.transform.SetParent(GlassBtnsParent, false);
            AssignGlassBtnData(GO, currentGlass, j);
            glassBtns.Add(GO);
        }
    }

    void AssignGlassBtnData(GameObject _currentGlassBtn, Microbe _currentMicrobeData, int _index)
    {
        if (_currentGlassBtn.GetComponent<GlassBtn>())
        {
            var currentGlassButton = _currentGlassBtn.GetComponent<GlassBtn>();
            _currentGlassBtn.GetComponent<Button>().onClick.AddListener(() => AssignDetailsOfTheMicrobe(_index, currentGlassButton));
            currentGlassButton.microbeName = _currentMicrobeData.microbeName;
            currentGlassButton.buttonText.text = _currentMicrobeData.microbeName;
            currentGlassButton.DetailsCgr = DetailsPage.GetComponent<CanvasGroup>();
        }
    }

    public void AssignDetailsOfTheMicrobe(int _index, GlassBtn _glassBtn)
    {
        var details = ApiData.microbeAssociatedInfo.microbe.microbeData;
        DetailsOfTheMicrobe.text = details[_index].description;
        timeStamp.text = "Observed At: " + details[_index].TimeStamp;
        imageInDetailsPage.texture = microbeImages[_index];
        _glassBtn.FunctionsToExecute();
    }

    public void DownloadImages()
    {
        var tempListOfMicrobeNames = new List<string>();
        for (int i = 0; i < ApiData.microbeAssociatedInfo.microbe.microbeData.Count; i++)
        {
            var currentMicrobeName = ApiData.microbeAssociatedInfo.microbe.microbeData[i].microbeName;
            tempListOfMicrobeNames.Add(currentMicrobeName);
        }
        ApiData.SetUrlAndGetImages(tempListOfMicrobeNames, GetFetchedImagesAndPopulateList);
    }

    void GetFetchedImagesAndPopulateList(List<Texture> _fetchedTextureList)
    {
        microbeImages = _fetchedTextureList;
        AssignImagesRespectedToGlass();
    }

    public void DownloadAssetBundles()
    {
        var tempListOfMicrobeNames = new List<string>();
        for (int i = 0; i < ApiData.microbeAssociatedInfo.microbe.microbeData.Count; i++)
        {
            var currentMicrobeName = ApiData.microbeAssociatedInfo.microbe.microbeData[i].microbeName;
            tempListOfMicrobeNames.Add(currentMicrobeName);
        }
        ApiData.SetUrlAndGetAssetBundles(tempListOfMicrobeNames, GetFetchedModelsAndPopulateList);
    }

    void GetFetchedModelsAndPopulateList(List<GameObject> _fetchedModelList)
    {
        microbeModels = _fetchedModelList;
        AssignModelRespectedToGlass();
        splashStartButton.onClick.Invoke();
    }

    void AssignModelRespectedToGlass()
    {
        for (int i = 0; i < microbeModels.Count; i++)
        {
            glassBtns[i].GetComponent<GlassBtn>().microbeModel = microbeModels[i];
        }
    }

    void AssignImagesRespectedToGlass()
    {
        for (int i = 0; i < microbeImages.Count; i++)
        {
            glassBtns[i].GetComponent<RawImage>().texture = microbeImages[i];
        }
    }
}

