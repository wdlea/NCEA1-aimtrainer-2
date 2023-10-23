using UnityEngine;

/// <summary>
/// Allows user to pause game
/// </summary>
public class Pauser : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) DoPause();//pause if esc key is pressed becuase that is common in games
    }

    public void DoPause(){
        if(!GameOverManager.IsGameOver)//only pasue if it isn't paused
            PauseManager.Pause();
    }
}
