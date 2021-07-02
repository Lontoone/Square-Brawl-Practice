using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//For test
public class LoadScene : MonoBehaviour
{
    public void LoadSceneByName(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }
}
