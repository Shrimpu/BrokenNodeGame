using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SceneButton : MonoBehaviour
{
    public AudioClip audioClip;
    AudioSource au;

    private void Awake()
    {
        au = GetComponent<AudioSource>();
    }

    private void Start()
    {
        au.clip = audioClip;
    }

    public void ChangeTheScene()
    {
        au.Play(0);
        ChangeScenes.LoadNextScene();
    }

    public void ReloadScene()
    {
        au.Play(0);
        ChangeScenes.ReloadScene();
    }

    public void Quit()
    {
        au.volume = 1000000; // it caps at 1 but this is cool too
        au.Play(0);
        ChangeScenes.Quit();
    }
}
