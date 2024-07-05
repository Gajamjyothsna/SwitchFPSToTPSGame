using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TPSGame.TPSGameDataModel;

namespace TPSGame
{
    public class SoundManager : MonoBehaviour
    {
        #region Singleton
        private static SoundManager _instance;

        public static SoundManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SoundManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Private Variables
        [Header("Sound Clips")]
        [SerializeField] private List<SoundClip> soundClips;
        private Dictionary<SoundType, AudioClip> _soundDict;
        #endregion

        #region Private Methods

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }

            _soundDict = new Dictionary<SoundType, AudioClip>();
            foreach (var soundClip in soundClips)
            {
                _soundDict[soundClip.soundType] = soundClip.audioClip;
            }
        }
        #endregion

        public void PlaySound(AudioSource audioSource, SoundType soundType)
        {
            Debug.Log("Soundtype :" + soundType);
            if (soundType == SoundType.None)
            {
                audioSource.clip = null;
            }
            if (_soundDict.ContainsKey(soundType))
            {
                audioSource.PlayOneShot(_soundDict[soundType]);
            }
            else
            {
                Debug.LogWarning("Sound type not found: " + soundType);
            }
        }

        public void PlayBackgroundMusic(AudioSource audioSource, SoundType soundType)
        {
            if (_soundDict.ContainsKey(soundType))
            {
                audioSource.clip = _soundDict[soundType];
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Background music type not found: " + soundType);
            }
        }

        public void StopSound(AudioSource _audioSource)
        {
            _audioSource.Stop();
        }

    }
}