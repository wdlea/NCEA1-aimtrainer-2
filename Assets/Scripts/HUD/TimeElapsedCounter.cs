using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
/// Displays the currently elapsed time to the user
///</summary>
public class TimeElapsedCounter : MonoBehaviour
{
    //the formatting to the time elapsed
    [SerializeField] StatisticDisplay _display;

    // Update is called once per frame
    void Update()
    {
        _display.DisplayStatistic(Statistics.SurvivalTime);//display the time elapsed
    }
}
