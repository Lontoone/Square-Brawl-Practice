using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

}
