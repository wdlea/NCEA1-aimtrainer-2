using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the main menu
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// The index of the game scene for loading.
    /// </summary>
    [SerializeField] int _gameSceneIndex;

    #region buttonCallbacks
    public void Quit(){
        Application.Quit();
    }

    public void Settings(){
        throw new NotImplementedException();
    }
    public void Play(){
        SceneManager.LoadScene(_gameSceneIndex);
    }
    #endregion
}
