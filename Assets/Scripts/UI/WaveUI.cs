using System;
using System.Collections;
using DG.Tweening;
using MyUtils;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WaveUI : MonoBehaviour
    {
        private int _waveCountdown = 30;

        [SerializeField] private Image _clockImage, _enemyImage, _waveImage;
        [SerializeField]private Text _waveText, _countdownText, _enemyText;

        private void Awake()
        {
            ShowUIElements();
            StartCountDown();
        }

        private void OnEnable()
        {
            EventManager.OnWaveComplete += StartNewWave;
        }

        private void OnDisable()
        {
            EventManager.OnWaveComplete -= StartNewWave;
        }

        private void StartNewWave()
        {
            StartCoroutine(NewWaveDelay());
        }

        private IEnumerator NewWaveDelay()
        {
            yield return BetterWaitForSeconds.Wait(2);
            ShowUIElements();
            StartCountDown();
            StopCoroutine(NewWaveDelay());
        }

        private void ShowUIElements()
        {
            
            _enemyText.gameObject.SetActive(true);
            _countdownText.gameObject.SetActive(true);
            _clockImage.gameObject.SetActive(true);
            _enemyImage.gameObject.SetActive(true);
            
            _waveText.gameObject.SetActive(true);
            _waveImage.gameObject.SetActive(true);
            _waveImage.DOFade(1, 0);
            _waveText.DOFade(1, 0);
            _waveText.text = "Wave " + PlayerPrefs.GetInt("Wave", 1);
            
            if(PlayerPrefs.HasKey("NumberOfEnemies"))
                _enemyText.text = PlayerPrefs.GetInt("NumberOfEnemies").ToString();
            else
            {
                int numOfEnemies = PlayerPrefs.GetInt("NumberOfEnemies") + 10;
                _enemyText.text = numOfEnemies.ToString();
            }
            _waveText.DOFade(0, 5f);
            _waveImage.DOFade(0, 5f).OnComplete(() => _waveImage.gameObject.SetActive(false));
        }

        private void HideUIElements()
        {
            _enemyText.gameObject.SetActive(false);
            _countdownText.gameObject.SetActive(false);
            _clockImage.gameObject.SetActive(false);
            _enemyImage.gameObject.SetActive(false);
        }

        private void StartCountDown()
        {
            InvokeRepeating(nameof(CountDown),0,1);
        }

        private void CountDown()
        {
            _countdownText.text = _waveCountdown.ToString();
            _waveCountdown--;

            if (_waveCountdown <= 0)
            {
                CancelInvoke(nameof(CountDown));
                _waveCountdown = 30;
                HideUIElements();
            }
        }
    }
}
