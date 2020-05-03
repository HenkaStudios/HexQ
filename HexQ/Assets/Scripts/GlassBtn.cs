using GoogleARCore.Examples.HelloAR;
using System.Collections;
using TMPro;
using UnityEngine;

public class GlassBtn : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public static CanvasGroup GalleryCgr;
    public CanvasGroup DetailsCgr;
    public GameObject microbeModel;
    public string microbeName;

    // Start is called before the first frame update
    private void Start()
    {
        if (!GalleryCgr)
        {
            GalleryCgr = transform.parent.parent.parent.parent.GetComponent<CanvasGroup>();
        }

    }

    public void FunctionsToExecute()
    {
        GoToDetailsPage();
        HelloARController.instance.SetARPrefab(microbeModel);
    }

    public void GoToDetailsPage()
    {
        StartCoroutine(CanvasGroupPercent(GalleryCgr, 0, 0.2f, true));
        DetailsCgr.gameObject.SetActive(true);
        StartCoroutine(CanvasGroupPercent(DetailsCgr, 1, 0.2f, false));
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
        if (animDisable)
        {
            if (Cgr.gameObject.activeInHierarchy)
            {
                Cgr.gameObject.SetActive(false);
            }
        }

        yield break;
    }
}
