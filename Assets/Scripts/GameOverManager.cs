using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Packages.Rider.Editor.UnitTesting;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] StatisticDisplay _scoreText;
    [SerializeField] StatisticDisplay _hitRateText;
    [SerializeField] StatisticDisplay _timeSurvivedText;
    [SerializeField] StatisticDisplay _targetCountText;
    [SerializeField] StatisticDisplay _accuracyText;


    void Start(){
        _scoreText.DisplayStatistic(Statistics.Score);
        _hitRateText.DisplayStatistic(Statistics.HitRate);
        _timeSurvivedText.DisplayStatistic(Statistics.SurvivalTime);
        _targetCountText.DisplayStatistic(Statistics.TargetsSpawned);
        _accuracyText.DisplayStatistic(Statistics.Accuracy);
    }
}

[Serializable] 
public struct StatisticDisplay{
    [SerializeField] Text _displayText;
    [SerializeField] string _textPrefix;
    [SerializeField] string _textSuffix;

    public void DisplayStatistic(string text){
        StringBuilder builder = new();

        builder.Append(_textPrefix);
        builder.Append(text);
        builder.Append(_textSuffix);

        _displayText.text = builder.ToString();
    }

    public void DisplayStatistic(IFormattable value){
        DisplayStatistic(value.ToString());
    }
}
