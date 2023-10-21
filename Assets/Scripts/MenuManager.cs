using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] int _gameSceneIndex;

    public void Quit(){
        Application.Quit();
    }

    public void Settings(){
        throw new NotImplementedException();
    }
    public void Play(){
        SceneManager.LoadScene(_gameSceneIndex);
    }
}
