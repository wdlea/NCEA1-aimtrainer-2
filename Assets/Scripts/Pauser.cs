using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pauser : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) DoPause();//pause if esc key is pressed becuase that is common in games
    }

    public void DoPause(){
        PauseManager.Pause();
    }
}
