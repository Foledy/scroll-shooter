using System;
using Settings;
using UnityEngine;
using UnityEngine.Serialization;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("AudioSource")] 
        [SerializeField] private AudioSource _effectsAudio;
        [SerializeField] private AudioSource _musicAudio;
        
        [Header("Clips")]
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private AudioClip _reloadSound;
        [SerializeField] private AudioClip _fireAttackSound;
        [SerializeField] private AudioClip _fireHitSound;
        [SerializeField] private AudioClip _katanaHitSound;
        [SerializeField] private AudioClip _katanaAttackSound;
        [SerializeField] private AudioClip _healSound;

        private static AudioManager _audioManager;

        private void Awake()
        {
            _audioManager = this;
        }

        private void Start()
        {
            SetAudioVolume(SettingsManager.MusicVolume, SettingsManager.EffectsVolume);
        }

        public static void SetAudioVolume(float musicVolume, float effectsVolume)
        {
            _audioManager._musicAudio.volume = musicVolume;
            _audioManager._effectsAudio.volume = effectsVolume;
        }
        
        public static void ClickSound() => PlaySound(_audioManager._clickSound);
        
        public static void FireAttackSound() => PlaySound(_audioManager._fireAttackSound);
        
        public static void FireHitSound() => PlaySound(_audioManager._fireHitSound);
        
        public static void KatanaAttackSound() => PlaySound(_audioManager._katanaAttackSound);
        
        public static void HealSound() => PlaySound(_audioManager._healSound);
        
        public static void KatanaHitSound() => PlaySound(_audioManager._katanaHitSound);
        
        public static void TakeDamageSound() => PlaySound(_audioManager._katanaHitSound);
        
        public static void ReloadSound() => PlaySound(_audioManager._reloadSound);
        
        private static void PlaySound(AudioClip clip) => _audioManager._effectsAudio.PlayOneShot(clip);
    }
}