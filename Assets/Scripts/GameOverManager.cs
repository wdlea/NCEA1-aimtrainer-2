using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static bool IsGameOver {get; private set;}

    public const int GAME_OVER_SCENE_INDEX = 3;
    [SerializeField] private int _gameSceneIndex = 1;
    [SerializeField] private int _menuSceneIndex = 0;

    [SerializeField] StatisticDisplay _scoreText;
    [SerializeField] StatisticDisplay _hitRateText;
    [SerializeField] StatisticDisplay _timeSurvivedText;
    [SerializeField] StatisticDisplay _targetCountText;
    [SerializeField] StatisticDisplay _accuracyText;

    void Start(){
        Time.timeScale = 0;
        _scoreText.DisplayStatistic(Statistics.Score);
        _hitRateText.DisplayStatistic(Statistics.HitRate);
        _timeSurvivedText.DisplayStatistic(Statistics.SurvivalTime);
        _targetCountText.DisplayStatistic(Statistics.TargetsSpawned);
        _accuracyText.DisplayStatistic(Statistics.Accuracy);
    }

    public static void GameOver(){
        IsGameOver = true;
        SceneManager.LoadScene(GAME_OVER_SCENE_INDEX, LoadSceneMode.Additive);
    }

    public async void Restart(){
        SceneManager.LoadScene(_gameSceneIndex);
        AsyncOperation op = SceneManager.UnloadSceneAsync(GAME_OVER_SCENE_INDEX);
        while(!op.isDone) await Task.Yield();
        Time.timeScale = 1;
        IsGameOver = false;
    }

    public async void MainMenu(){
        SceneManager.LoadScene(_menuSceneIndex);
        AsyncOperation op = SceneManager.UnloadSceneAsync(GAME_OVER_SCENE_INDEX);
        while(!op.isDone) await Task.Yield();
        Time.timeScale = 1;
        IsGameOver = false;
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
