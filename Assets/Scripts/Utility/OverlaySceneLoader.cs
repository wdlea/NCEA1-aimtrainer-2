using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles loading the music scene
/// </summary>
public class OverlaySceneLoader : MonoBehaviour
{
    static HashSet<int> _loadedScenes = new();//store the loaded scenes in a static set to determine what scenes are loaded

    [SerializeField] int _sceneIndex;//the scene index to load
    [SerializeField] bool _thereCanOnlyBeOne;
    // Start is called before the first frame update
    void Start()
    {
        //only load if there are multiple allowed or the scene has not been loaded
        if(!_thereCanOnlyBeOne || !_loadedScenes.Contains(_sceneIndex))
            SceneManager.LoadScene(_sceneIndex, LoadSceneMode.Additive);//not using LoadSceneASync becuase that might create a data race if other loaders are allowed to load the same scene simultaneously, also becuase Unity's LoadSceneAsync doesn't support await
        _loadedScenes.Add(_sceneIndex);
    }
}
