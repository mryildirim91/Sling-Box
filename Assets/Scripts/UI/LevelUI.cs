using System.Collections;
using MyUtils;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelUI : MonoBehaviour
    {
        private int _score, _level;
        [SerializeField] private Text _scoreText, _levelText;
        [SerializeField] private GameObject _bossPanel, _gameOverPanel, _levelCompletePanel;
        [SerializeField] private Image _clockImage, _enemyImage;
        [SerializeField] private Text _enemyNumText, _countDownText;

        private void Awake()
        {
            Time.timeScale = 0;
            _level = PlayerPrefs.GetInt("Level") + 1;
            _levelText.text = "Level " + _level;
            _score = PlayerPrefs.GetInt("Score");
            _scoreText.text = "Score " + PlayerPrefs.GetInt("Score");
        }

        private void OnEnable()
        {
            EventManager.OnExplode += UpdateScore;
            EventManager.OnSendBoss += OpenBossPanel;
            EventManager.OnGameOver += OpenGameOverPanel;
            EventManager.OnLevelComplete += OpenLevelComplete;
        }

        private void OnDisable()
        {
            EventManager.OnExplode -= UpdateScore;
            EventManager.OnSendBoss -= OpenBossPanel;
            EventManager.OnGameOver -= OpenGameOverPanel;
            EventManager.OnLevelComplete -= OpenLevelComplete;
        }

        private void UpdateScore()
        {
            _score = _score + Random.Range(10, 16);
            _scoreText.text = "Score " + _score;
            PlayerPrefs.SetInt("Score", _score);
        }

        private void OpenBossPanel()
        {
            _bossPanel.SetActive(true);
            StartCoroutine(CloseBossPanel());
        }

        private IEnumerator CloseBossPanel()
        {
            yield return BetterWaitForSeconds.Wait(3);
            _bossPanel.SetActive(false);
            _clockImage.gameObject.SetActive(true);
            _enemyImage.gameObject.SetActive(true);
            _enemyNumText.gameObject.SetActive(true);
            _countDownText.gameObject.SetActive(true);
            _enemyNumText.text = "1";
            
            int countDown = 60;
            
            while (countDown > 0)
            {
                yield return BetterWaitForSeconds.Wait(1);
                countDown--;
                _countDownText.text = countDown.ToString();
            }
            _clockImage.gameObject.SetActive(false);
            _enemyImage.gameObject.SetActive(false);
            _enemyNumText.gameObject.SetActive(false);
            _countDownText.gameObject.SetActive(false);
        }

        private void OpenGameOverPanel()
        {
            _gameOverPanel.SetActive(true);
        }

        private void OpenLevelComplete()
        {
            StartCoroutine(DelayLevelCompletePanel());
        }

        private IEnumerator DelayLevelCompletePanel()
        {
            yield return BetterWaitForSeconds.Wait(1);
            _levelCompletePanel.SetActive(true);
        }
    }
}
