using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool _gameOver, _levelComplete;
    private int _numOfWaves = 1;
    private int _level;
    
    private void Awake()
    {
        Instance = this;
        //PlayerPrefs.DeleteAll();
        GameAnalytics.Initialize();
        
        if (PlayerPrefs.HasKey("Wave"))
        {
            _numOfWaves = PlayerPrefs.GetInt("Wave");
        }

        _level = PlayerPrefs.GetInt("Level") + 1;
        
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level " + _level);
    }
    
    public void GameOver()
    {
        _gameOver = true;
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level " + _level);
    }

    public void WavePassed()
    {
        _numOfWaves++;
        PlayerPrefs.SetInt("Wave", _numOfWaves);
        PlayerPrefs.SetInt("NumberOfEnemies", PlayerPrefs.GetInt("NumberOfEnemies") + 3);
    }

    public void LevelComplete()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level " + _level);
        _levelComplete = true;
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        PlayerPrefs.SetInt("Wave", 1);
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void RestartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void PauseGame(bool paused)
    {
        if (paused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public bool StopPlaying()
    {
        if (_gameOver || _levelComplete)
            return true;
        
        return false;
    }

    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
