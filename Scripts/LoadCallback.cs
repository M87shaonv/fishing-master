using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCallback : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadSceneAsync(Loader.currentScene));
    }

    private IEnumerator LoadSceneAsync(SceneType sceneType)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneType.ToString());

        // 等待场景加载完毕
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}