using System;
using System.Collections.Generic;
using UnityEngine;
using WordVista.Save;

namespace WordVista.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource ambientSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip[] worldMusicClips; // 8 world tracks
        [SerializeField] private AudioClip[] worldAmbientClips;
        [SerializeField] private AudioClip buttonClickClip;
        [SerializeField] private AudioClip letterSelectClip;
        [SerializeField] private AudioClip wordCorrectClip;
        [SerializeField] private AudioClip wordInvalidClip;
        [SerializeField] private AudioClip levelCompleteClip;
        [SerializeField] private AudioClip hintPurchasedClip;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ApplySettings();
        }

        public void ApplySettings()
        {
            var data = SaveManager.Instance.Data;
            if (musicSource != null)
            {
                musicSource.mute = !data.musicEnabled;
                musicSource.volume = data.musicVolume;
            }
            if (ambientSource != null)
            {
                ambientSource.mute = !data.musicEnabled;
                ambientSource.volume = data.musicVolume * 0.7f;
            }
            if (sfxSource != null)
            {
                sfxSource.mute = !data.sfxEnabled;
                sfxSource.volume = data.sfxVolume;
            }
        }

        public void PlayWorldMusic(int worldId)
        {
            if (worldMusicClips == null || worldMusicClips.Length == 0) return;
            int idx = Mathf.Clamp(worldId - 1, 0, worldMusicClips.Length - 1);
            var clip = worldMusicClips[idx];
            if (clip != null && musicSource != null && musicSource.clip != clip)
            {
                musicSource.clip = clip;
                musicSource.loop = true;
                musicSource.Play();
            }

            if (worldAmbientClips != null && idx < worldAmbientClips.Length && ambientSource != null)
            {
                var amb = worldAmbientClips[idx];
                if (amb != null && ambientSource.clip != amb)
                {
                    ambientSource.clip = amb;
                    ambientSource.loop = true;
                    ambientSource.Play();
                }
            }
        }

        public void PlaySFX(AudioClip clip, float pitch = 1.0f)
        {
            if (clip != null && sfxSource != null && SaveManager.Instance.Data.sfxEnabled)
            {
                sfxSource.pitch = pitch;
                sfxSource.PlayOneShot(clip);
            }
        }

        public void PlayButtonClick() => PlaySFX(buttonClickClip);
        public void PlayWordCorrect() => PlaySFX(wordCorrectClip);
        public void PlayWordInvalid() => PlaySFX(wordInvalidClip);
        public void PlayLevelComplete() => PlaySFX(levelCompleteClip);
        public void PlayHintPurchased() => PlaySFX(hintPurchasedClip);

        public void TriggerHapticFeedback()
        {
            if (SaveManager.Instance.Data.hapticsEnabled)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                Handheld.Vibrate();
#endif
            }
        }
    }
}
