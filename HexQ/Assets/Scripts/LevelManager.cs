using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadScene(int _index)
    {
        StartCoroutine(LoadYourAsyncScene(_index));
    }

    IEnumerator LoadYourAsyncScene(int _index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_index);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
