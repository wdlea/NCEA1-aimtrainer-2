using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeElapsedCounter : MonoBehaviour
{
    [SerializeField] StatisticDisplay _display;

    // Update is called once per frame
    void Update()
    {
        _display.DisplayStatistic(Statistics.SurvivalTime);
    }
}
