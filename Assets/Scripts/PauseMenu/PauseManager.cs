using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the pause scene
/// </summary>
public class PauseManager : MonoBehaviour
{
    #region indexesForSwitchingScenes
    private const int PAUSE_SCENE_INDEX = 2;
    [SerializeField] private int _menuSceneIndex;
    [SerializeField] private int _gameSceneIndex;

    #endregion
    /// <summary>
    /// Whether the game is currently paused
    /// </summary>
    public static bool IsPaused {get; private set;}

    /// <summary>
    /// Pauses the game if it is not paused already
    /// </summary>
    public static void Pause(){
        if(IsPaused) return;//early return if it is already pauses to stop the pause menus "stacking"
        IsPaused = true;//Set paused to true
        Time.timeScale = 0;//stop time to actually pause the game
        SceneManager.LoadScene(PAUSE_SCENE_INDEX, LoadSceneMode.Additive);//then load the pause scene
    }

    /// <summary>
    /// Unpauses the game
    /// </summary>
    public void Continue(){
        SceneManager.UnloadSceneAsync(PAUSE_SCENE_INDEX);//unload this scene
        Time.timeScale = 1;//reset time
        IsPaused = false;//and set IsPaused to false
    }

    /// <summary>
    /// Restarts the game
    /// </summary>
    public async void Restart(){
        AsyncOperation op = SceneManager.UnloadSceneAsync(PAUSE_SCENE_INDEX);//unload this scene
        while(!op.isDone) await Task.Yield();//wait for this scene to finish unloading
        SceneManager.LoadScene(_gameSceneIndex);//restart the game scene
        Time.timeScale = 1;//restart time
        IsPaused = false;//and set paused to false
    }

    /// <summary>
    /// Takes the user to the main menu
    /// </summary>
    public async void MainMenu(){
        SceneManager.LoadScene(_menuSceneIndex);//load the menu scene
        AsyncOperation op = SceneManager.UnloadSceneAsync(PAUSE_SCENE_INDEX);//unload this one
        while(!op.isDone) await Task.Yield();
        Time.timeScale = 1;//then reset time
        IsPaused = false;
    }
}
