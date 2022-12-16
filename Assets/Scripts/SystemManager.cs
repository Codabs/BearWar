using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneControllerMD;
using UnityEngine.SceneManagement;

public class SystemManager : Singleton<SystemManager>
{
    private void Start()
    {
        // start Menu scene
        StartCoroutine(loadScene());
    }
    IEnumerator loadScene()
    {
        Time.timeScale = 0;
        AsyncOperation LoadingScene = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        while (!LoadingScene.isDone) { yield return null; }
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        Time.timeScale = 1;
    }
}
