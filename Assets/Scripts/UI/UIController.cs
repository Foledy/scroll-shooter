using System;
using Audio;
using Save;
using Settings;
using Time;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [Header("Canvas")]
        [SerializeField] private Canvas _mainMenuCanvas;
        [SerializeField] private Canvas _settingsCanvas;
        [SerializeField] private Canvas _pauseCanvas;
        
        [Header("Volume Settings")]
        [SerializeField] private SaveLoadManager _saveLoad;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _effectVolumeSlider;

        [Header("Other")] 
        [SerializeField] private bool _isGameResultScene;
        [SerializeField] private bool _isMainMenuScene;

        [SerializeField] private Text _scoreText;

        private Canvas _currentCanvas;

        private void Start()
        {
            if (_isGameResultScene == true)
            {
                _scoreText.text = PlayerController.Score.ToString();
            }
            else if (_isMainMenuScene == true)
            {
                _currentCanvas = _mainMenuCanvas;
            
                LoadSettings();
            }
        }

        public void LoadScene(string sceneName)
        {
            PlayClickSound();
            
            SceneManager.LoadScene(sceneName);
        }

        public void RestartLevel()
        {
            ResumeGame();
            
            LoadScene("LevelScene");
        }

        public void BackToMenu()
        {
            _currentCanvas = _mainMenuCanvas;
            
            TimeManager.ResumeGame();
            
            LoadScene("MainMenuScene");
        }

        public void QuitGame()
        {
            PlayClickSound();
            
            Application.Quit();
        }

        public void SaveSettings()
        {
            _saveLoad.SaveSettings();
        }

        public void UpdateMusicVolume(Slider slider)
        {
            SettingsManager.MusicVolume = slider.value;
            
            AudioManager.SetAudioVolume(_musicVolumeSlider.value, _effectVolumeSlider.value);
        }

        public void UpdateEffectsVolume(Slider slider)
        {
            SettingsManager.EffectsVolume = slider.value;
            
            AudioManager.SetAudioVolume(_musicVolumeSlider.value, _effectVolumeSlider.value);
        }

        public void EnableCanvas(Canvas canvas)
        {
            canvas.gameObject.SetActive(true);

            _currentCanvas = canvas;
        }

        public void DisableCanvas(Canvas canvas)
        {
            _currentCanvas = _mainMenuCanvas;
            
            canvas.gameObject.SetActive(false);
        }

        public void ChangeCanvas(Canvas newCanvas)
        {
            DisableCanvas(_currentCanvas);
            
            EnableCanvas(newCanvas);
            
            SaveSettings();
            
            PlayClickSound();
        }

        public void PauseGame()
        {
            EnableCanvas(_pauseCanvas);
            
            PlayClickSound();
            
            TimeManager.StopGame();
        }

        public void ResumeGame()
        {
            DisableCanvas(_pauseCanvas);
            
            PlayClickSound();

            TimeManager.ResumeGame();
        }

        private void LoadSettings()
        {
            _saveLoad.LoadSettings();

            _musicVolumeSlider.value = SettingsManager.MusicVolume;
            _effectVolumeSlider.value = SettingsManager.EffectsVolume;
        }

        public void PlayClickSound() => AudioManager.ClickSound();
    }
}