using UnityEngine;
using UnityEngine.SceneManagement;

public class OverlaySceneLoader : MonoBehaviour
{
    [SerializeField] int _sceneIndex;
    [SerializeField] bool _thereCanOnlyBeOne;
    // Start is called before the first frame update
    void Start()
    {
        if(!_thereCanOnlyBeOne || !SceneManager.GetSceneByBuildIndex(_sceneIndex).isLoaded)
            SceneManager.LoadScene(_sceneIndex, LoadSceneMode.Additive);//not using LoadSceneASync becuase that might create a data race if other loaders are allowed to load the same scene simultaneously, also becuase Unity's LoadSceneAsync doesn't support await
    }
}
