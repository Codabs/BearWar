using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace SceneControllerMD
{
    /// <summary>
    /// Scene controller for avoid "Don't destroy on load" with your own scene manager.
    /// </summary>
    public class SceneController : SingletonND<SceneController>
    {
        // override singleton awake fonction to make the controller work.
        private void Awake() { }

        public void LoadScene(Scene NewScene) => StartCoroutine(LoadScene_(NewScene));
        public void LoadScene(string NewScene) => StartCoroutine(LoadScene_(NewScene));
        IEnumerator LoadScene_(Scene NewScene)
        {
            Time.timeScale = 0;
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneAt(0)) {
                AsyncOperation UnloadingScene = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                while (!UnloadingScene.isDone) { yield return null; }
            }
            Debug.Log(NewScene.name);
            AsyncOperation LoadingScene = SceneManager.LoadSceneAsync(NewScene.name, LoadSceneMode.Additive);
            while (!LoadingScene.isDone) { yield return null; }
            if (SceneManager.GetActiveScene() != NewScene) SceneManager.SetActiveScene(NewScene);
            Time.timeScale = 1;
        }
        IEnumerator LoadScene_(string NewScene)
        {
            Time.timeScale = 0;
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneAt(0))
            {
                AsyncOperation UnloadingScene = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                while (!UnloadingScene.isDone) { yield return null; }
            }
            AsyncOperation LoadingScene = SceneManager.LoadSceneAsync(NewScene, LoadSceneMode.Additive);
            while (!LoadingScene.isDone) { yield return null; }
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName(NewScene)) SceneManager.SetActiveScene(SceneManager.GetSceneByName(NewScene));
            Time.timeScale = 1;
        }

        public void ReloadScene() => StartCoroutine(ReloadScene_());
        IEnumerator ReloadScene_()
        {
            Time.timeScale = 0;
            Scene scene = SceneManager.GetActiveScene();
            int buildIndexScene = scene.buildIndex;
            AsyncOperation UnloadingScene = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            while (!UnloadingScene.isDone) { yield return null; }
            AsyncOperation LoadingScene = SceneManager.LoadSceneAsync(buildIndexScene, LoadSceneMode.Additive);
            while (!LoadingScene.isDone) { yield return null; }
            if (SceneManager.GetActiveScene() != scene) SceneManager.SetActiveScene(scene);
            Time.timeScale = 1;
        }
        public void MoveGameObjectToNonDestroyScene(GameObject gameObject)
        {
            if (SceneManager.GetSceneAt(0).isLoaded)
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneAt(0));
            else StartCoroutine(MoveGameObjectToNonDestroyScene_(gameObject));
        }
        IEnumerator MoveGameObjectToNonDestroyScene_(GameObject gameObject)
        {
            AsyncOperation LoadingScene = SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
            while (!LoadingScene.isDone) { yield return null; }
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneAt(0)) SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneAt(0));
        }
    }
}
