using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Audio;
using Settings;
using UnityEngine;

namespace Save
{
    public class SaveLoadManager : MonoBehaviour
    {
        private  string _filePath;

        private void Start()
        {
            _filePath = Application.persistentDataPath + "/ScrollShooter.settings";
        }

        public void SaveSettings()
        {
            var formatter = new BinaryFormatter();
            using var fileStream = new FileStream(_filePath, FileMode.Create);
            var save = new Save();
                
            save.SaveData(SettingsManager.MusicVolume, SettingsManager.EffectsVolume);
                
            formatter.Serialize(fileStream, save);
        }

        public void LoadSettings()
        {
            if (File.Exists(_filePath) == false)
            {
                SettingsManager.MusicVolume = 1;
                SettingsManager.EffectsVolume = 1;
                
                return;
            }

            var formatter = new BinaryFormatter();
            using var fileStream = new FileStream(_filePath, FileMode.Open);

            var save = (Save)formatter.Deserialize(fileStream);

            SettingsManager.MusicVolume = save.VolumeData.Music;
            SettingsManager.EffectsVolume = save.VolumeData.Effects;
            
            AudioManager.SetAudioVolume(save.VolumeData.Music, save.VolumeData.Effects);
        }
    }
    
    [System.Serializable]
    public class Save
    {
        [System.Serializable]
        public struct VolumeSettingsData
        {
            public float Music, Effects;

            public VolumeSettingsData(float music, float effects)
            {
                Music = music;
                Effects = effects;
            }
        }

        public VolumeSettingsData VolumeData;

        public void SaveData(float musicVolume, float effectsVolume)
        {
            VolumeData = new VolumeSettingsData(musicVolume, effectsVolume);
        }
    }
}