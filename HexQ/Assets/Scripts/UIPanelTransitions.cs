using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelTransitions : MonoBehaviour
{

    public List<PanelsAndBtns> panelsAndBtns= new List<PanelsAndBtns>();
    public GameObject[] elements;
    [HideInInspector]
    public CanvasGroup canvasGroup;

    public float FadeInTime = 0.4f, FadeOutTime = 0.4f;

    private List<int> elementsWithQRTrans = new List<int>();
    private GameObject currentPanel;
    private GameObject currentAnimObj;
    bool isChanging = false;
    bool canClick = true;
    int inc1;

    void Start()
    {
        AddListenersToBtns();
    }

    void AddListenersToBtns()
    {
        for (int i = 0; i < panelsAndBtns.Count; i++)
        {
            int j = i;
            if (panelsAndBtns[j].btn)
                panelsAndBtns[j].btn.onClick.AddListener(() => ChangeToPanel(j));

            for (int k = 0; k < panelsAndBtns[j].ElementsToBeActive.Count; k++)
            {
                if (panelsAndBtns[j].ElementsToBeActive[k])
                {
                    if (panelsAndBtns[j].ElementsToBeActive[k].name == "QRCodeTransition")
                    {
                        elementsWithQRTrans.Add(j);
                    }
                }
            }
        }

    }
    private void Update()
    {
        #region Unused Code

        /*  for (int i = 0; i < elements.Length; i++)
          {
              if (!elements[i].GetComponent<CanvasGroup>() || !elements[i].GetComponent<Animator>())
                  return;

              if (elements[i].GetComponent<CanvasGroup>().alpha ==0f)
              {
                  elements[i].GetComponent<Animator>().enabled = true;
              }
              if (elements[i].GetComponent<CanvasGroup>().alpha == 1)
              {
                  elements[i].GetComponent<Animator>().enabled = false;
              }

              if (elements[i].GetComponent<CanvasGroup>().alpha == 1f)
              {
                  elements[i].GetComponent<CanvasGroup>().interactable = true;

              }
              else
              {

                  elements[i].GetComponent<CanvasGroup>().interactable = false;
              }
          }*/
        #endregion
    }
    void ChangeToPanel(int _index)
    {
        if (!canClick)
            return;

        if (_index == 6)
        {
            if (canvasGroup)
                canvasGroup.enabled = true;
            Invoke("CloseCGR", FadeInTime + FadeOutTime);

        }
        else
        {
            foreach (var item in elementsWithQRTrans)
            {
                if (item == _index)
                {
                    if (canvasGroup)

                        canvasGroup.enabled = true;
                    break;
                }
                else
                {
                    if (canvasGroup)
                        canvasGroup.enabled = false;

                }
            }
        }

        if (isChanging)
            return;

        currentAnimObj = panelsAndBtns[_index].TransitionAnimObj;


        if (!currentAnimObj)
            StartCoroutine(PlayScreenTransitionAndEnablePanel_2(_index));
        else
            StartCoroutine(PlayScreenTransitionAndEnablePanel(_index));
    }

    void CloseCGR()
    {
        if (canvasGroup)

            canvasGroup.enabled = false;
    }

    void DisableAllOtherPanels(int _index)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].activeInHierarchy)
            {
                elements[i].SetActive(false);
            }
        }

        for (int j = 0; j < panelsAndBtns[_index].ElementsToBeActive.Count; j++)
        {
            if (panelsAndBtns[_index].ElementsToBeActive[j])
            {
                if (!panelsAndBtns[_index].ElementsToBeActive[j].activeInHierarchy)
                    panelsAndBtns[_index].ElementsToBeActive[j].SetActive(true);
            }

        }
    }

    IEnumerator PlayScreenTransitionAndEnablePanel(int _index)
    {
        isChanging = true;
        yield return new WaitForSeconds(0.25f);
        DisableAllOtherPanels(_index);
        isChanging = false;
        if (!canClick)
            canClick = true;
    }
    IEnumerator PlayScreenTransitionAndEnablePanel_2(int _index)
    {
        isChanging = true;

        //yield return Disabler(FadeOutTime, _index);
        //yield return Enabler(_index, FadeInTime);
        StartCoroutine(Disabler(FadeOutTime, _index));
        StartCoroutine(Enabler(_index, FadeInTime));

        isChanging = false;
        if (!canClick)
            canClick = true;
        yield break;

    }

    IEnumerator Disabler(float fDTime, int _index)
    {

        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i])
            {
                var data = panelsAndBtns[_index].ElementsToBeActive.Where(x => x.gameObject == elements[i]).SingleOrDefault();
                if (elements[i] != data)
                {
                    for (int j = 0; j < elements[i].GetComponentsInParent<CanvasGroup>().Length; j++)
                    {

                        if (elements[i].GetComponentsInParent<CanvasGroup>()[j])
                        {
                            if (elements[i].GetComponentsInParent<CanvasGroup>()[j].alpha > 0.5f)
                                StartCoroutine(CanvasGroupPercent(elements[i].GetComponentsInParent<CanvasGroup>()[j], 0, fDTime, false));
                        }
                    }
                }

            }
        }

        yield return new WaitForSeconds(fDTime);

        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i])
            {
                var data = panelsAndBtns[_index].ElementsToBeActive.Where(x => x.gameObject == elements[i]).SingleOrDefault();
                if (elements[i] != data)
                {
                    if (elements[i].activeInHierarchy)
                        elements[i].SetActive(false);

                }

            }
        }
    }

    IEnumerator Enabler(int _index, float FDTime)
    {

        for (int i = 0; i < panelsAndBtns[_index].ElementsToBeActive.Count; i++)
        {
            if (panelsAndBtns[_index].ElementsToBeActive[i])
            {
                for (int j = 0; j < panelsAndBtns[_index].ElementsToBeActive[i].GetComponentsInParent<CanvasGroup>().Length; j++)
                {
                    if (panelsAndBtns[_index].ElementsToBeActive[i].GetComponentsInParent<CanvasGroup>()[j])
                    {
                        panelsAndBtns[_index].ElementsToBeActive[i].GetComponentsInParent<CanvasGroup>()[j].alpha = 0;
                    }
                }
                if (!panelsAndBtns[_index].ElementsToBeActive[i].activeInHierarchy)
                    panelsAndBtns[_index].ElementsToBeActive[i].SetActive(true);
            }

        }

        yield return new WaitForSeconds(FDTime);

        for (int i = 0; i < panelsAndBtns[_index].ElementsToBeActive.Count; i++)
        {
            if (panelsAndBtns[_index].ElementsToBeActive[i])
            {
                for (int j = 0; j < panelsAndBtns[_index].ElementsToBeActive[i].GetComponentsInParent<CanvasGroup>().Length; j++)
                {
                    if (panelsAndBtns[_index].ElementsToBeActive[i].GetComponentsInParent<CanvasGroup>()[j])
                        StartCoroutine(CanvasGroupPercent(panelsAndBtns[_index].ElementsToBeActive[i].GetComponentsInParent<CanvasGroup>()[j], 1, FDTime, false));
                }

            }
        }

        yield return null;
    }

    public IEnumerator CanvasGroupPercent(CanvasGroup Cgr, float targetVal, float duration, bool animDisable)
    {
        float diffAmt = (targetVal - Cgr.alpha);
        float counter = 0;
        while (counter < duration)
        {
            float fillAmt = Cgr.alpha + (Time.deltaTime * diffAmt) / duration;
            Cgr.alpha = fillAmt;

            counter += Time.deltaTime;
            yield return null;
        }
        Cgr.alpha = targetVal;
        yield break;
    }

    #region Unused Code 

    //IEnumerator Disabler(float fDTime, int _index)
    //{

    //    //for (int i = 0; i < elements.Length; i++)
    //    //{
    //    //    if (elements[i])
    //    //    {
    //    //        for (int k = 0; k < panelsAndBtns[_index].ElementsToBeActive.Length; k++)
    //    //        {
    //    //            for (int j = 0; j < elements[i].GetComponentsInParent<RectTransform>().Length; j++)
    //    //            {

    //    //                if (elements[i] != panelsAndBtns[_index].ElementsToBeActive[k])
    //    //                {

    //    //                    if (elements[i].GetComponentsInParent<RectTransform>()[j])
    //    //                        StartCoroutine(CanvasGroupPercent(elements[i].GetComponentsInParent<RectTransform>()[j], Vector3.one * 0.01f, fDTime));
    //    //                }
    //    //            }
    //    //        }
    //    //    }
    //    //}
    //    for (int i = 0; i < elements.Length; i++)
    //    {
    //        if (elements[i])
    //        {
    //            var data = panelsAndBtns[_index].ElementsToBeActive.Where(x => x.gameObject == elements[i]).SingleOrDefault();
    //            if (elements[i] != data)
    //            {
    //                if (elements[i].GetComponent<RectTransform>())
    //                    StartCoroutine(RectTransformPercent(elements[i].GetComponent<RectTransform>(), Vector3.one * 0.01f, fDTime));

    //            }
    //        }
    //    }
    //    yield return new WaitForSeconds(fDTime);

    //    for (int i = 0; i < elements.Length; i++)
    //    {
    //        if (elements[i])
    //        {
    //            var data = panelsAndBtns[_index].ElementsToBeActive.Where(x => x.gameObject == elements[i]).SingleOrDefault();
    //            if (elements[i] != data)
    //                elements[i].SetActive(false);
    //        }
    //    }
    //}

    //IEnumerator Enabler(int _index, float FDTime)
    //{

    //    for (int i = 0; i < panelsAndBtns[_index].ElementsToBeActive.Length; i++)
    //    {
    //        if (panelsAndBtns[_index].ElementsToBeActive[i])
    //        {

    //            panelsAndBtns[_index].ElementsToBeActive[i].SetActive(true);
    //        }

    //    }

    //    yield return new WaitForSeconds(FDTime);

    //    for (int i = 0; i < panelsAndBtns[_index].ElementsToBeActive.Length; i++)
    //    {
    //        if (panelsAndBtns[_index].ElementsToBeActive[i])
    //        {
    //            if (panelsAndBtns[_index].ElementsToBeActive[i].GetComponent<RectTransform>())
    //                StartCoroutine(RectTransformPercent(panelsAndBtns[_index].ElementsToBeActive[i].GetComponent<RectTransform>(), Vector3.one, FDTime));
    //        }
    //    }


    //    yield return null;
    //}

    //public IEnumerator RectTransformPercent(RectTransform Cgr, Vector3 targetVal, float duration)
    //{
    //    float diffAmt = (targetVal.x - Cgr.localScale.x);
    //    float counter = 0;
    //    while (counter < duration)
    //    {
    //        float fillAmt = Cgr.localScale.x + (Time.deltaTime * diffAmt) / duration;
    //        Cgr.localScale = Vector3.one * fillAmt;

    //        counter += Time.deltaTime;
    //        yield return null;
    //    }
    //    Cgr.localScale = targetVal;
    //    yield break;
    //}
    #endregion

    void OnDisable()
    {
        for (int i = 0; i < panelsAndBtns.Count; i++)
        {
            int j = i;
            if (panelsAndBtns[j].btn)
                panelsAndBtns[j].btn.onClick.RemoveAllListeners();
        }
    }
}

[System.Serializable]
public class PanelsAndBtns
{
    public List<GameObject> ElementsToBeActive= new List<GameObject>();
    [HideInInspector]
    public GameObject TransitionAnimObj;
    public Button btn;
}