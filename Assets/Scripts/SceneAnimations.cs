using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAnimations : MonoBehaviour
{
    public Animation anime;
    public AnimationClip clip;

    private void Awake()
    {
        ChangeScenes.loadNext += CloseScene;
    }

    private void CloseScene()
    {
        anime.Play("LevelClear");
    }

    public void LoadNextLevel()
    {
        print("Hello");
        ChangeScenes.LoadNextScene();
    }
}
