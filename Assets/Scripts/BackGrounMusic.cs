using System.Collections;
using System.Collections.Generic;
using TPSGame;
using UnityEngine;

namespace TPSGame
{
    public class BackGroundMusic : MonoBehaviour
    {
        private AudioSource audioSource;
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (TPSGameManager.Instance.CurrentGameState == TPSGameDataModel.GameState.GameOver) audioSource.Stop();
            PlayBackgroundMusic();
        }

        public void PlayBackgroundMusic()
        {
            // Assuming SoundManager has a method called PlayClip which takes an AudioSource and a clip name
            SoundManager.Instance.PlayBackgroundMusic(audioSource, TPSGameDataModel.SoundType.BackGroundMusic);
        }
    }
}