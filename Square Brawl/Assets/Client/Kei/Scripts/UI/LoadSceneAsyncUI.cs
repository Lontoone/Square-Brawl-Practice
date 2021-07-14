using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LoadSceneAsyncUI : MonoBehaviour
{
    public string sceneName;
    public RawImage screenImage;
    public void LoadScene()
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        //SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        screenImage.gameObject.SetActive(true);
    }


    private void OnDisable()
    {
        //close scene when disable:
        SceneManager.UnloadSceneAsync(sceneName);

    }
}
