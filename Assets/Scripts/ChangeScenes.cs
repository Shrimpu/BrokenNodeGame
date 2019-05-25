using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ChangeScenes
{
    public delegate void LoadScene();
    public static LoadScene loadingAnyScene;
    public static LoadScene loadNext;
    public static LoadScene reload;
    public static LoadScene GameQuit;

    public static void LoadNextScene()
    {
        loadNext?.Invoke();
        loadingAnyScene?.Invoke();
        WireHandler.Reset();
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

    public static void ReloadScene()
    {
        reload?.Invoke();
        loadingAnyScene?.Invoke();
        WireHandler.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void Quit()
    {
        GameQuit?.Invoke();
        Application.Quit();
    }
}
