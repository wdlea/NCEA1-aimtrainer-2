using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private const int PAUSE_SCENE_INDEX = 2;
    [SerializeField] private int _menuSceneIndex;

    public static bool IsPaused {get; private set;}

    public static void Pause(){
        if(IsPaused) return;
        IsPaused = true;
        Time.timeScale = 0;
        SceneManager.LoadScene(PAUSE_SCENE_INDEX, LoadSceneMode.Additive);
    }

    public void Continue(){
        SceneManager.UnloadSceneAsync(PAUSE_SCENE_INDEX);
        Time.timeScale = 1;
        IsPaused = false;
    }

    public async void Restart(){
        AsyncOperation op = SceneManager.UnloadSceneAsync(PAUSE_SCENE_INDEX);
        while(!op.isDone) await Task.Yield();
        Time.timeScale = 1;
        IsPaused = false;
    }

    public async void MainMenu(){
        SceneManager.LoadScene(_menuSceneIndex);
        AsyncOperation op = SceneManager.UnloadSceneAsync(PAUSE_SCENE_INDEX);
        while(!op.isDone) await Task.Yield();
        Time.timeScale = 1;
        IsPaused = false;
    }
}
