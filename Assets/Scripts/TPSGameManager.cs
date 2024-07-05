using System;
using System.Collections;
using UnityEngine;
using static TPSGame.TPSGameDataModel;

namespace TPSGame
{
    public class TPSGameManager : MonoBehaviour
    {
        #region Singleton
        private static TPSGameManager _instance;

        public static TPSGameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<TPSGameManager>();
                }
                return _instance;
            }
        }
        #endregion

        #region Private Variables
        [Header("Time Duration Value")]
        [SerializeField] private float timeDuration = 5f;
        [Header("PopUp Messages")]
        [SerializeField] private string _navigationContent;
        [Header("Camera Components")]
        [SerializeField] private GameObject _tpsCamera;
        [SerializeField] private GameObject _fpsCamera;
        [SerializeField] private GameObject _tpsVirtualCamera;
        [Header("SpawnController")]
        [SerializeField] private SpawnController _spawnController;

        private bool isTPS = false;
        public GameState CurrentGameState { get; private set; } = GameState.Playing;
        public Action<GameState> OnGameStateChanged;
        #endregion

        #region MonoBehaviour Methods
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
        }
        private void Start ()
        {
            StartCoroutine(CheckTimeDurationToEnablePopUpMessage());
        }
        #endregion

        #region Private Methods
        private IEnumerator CheckTimeDurationToEnablePopUpMessage()
        {
            float elapsedTime = 0f;

            // Loop until elapsedTime reaches timeDuration
            while (elapsedTime < timeDuration)
            {
                elapsedTime += Time.deltaTime;
                yield return null; // Yield execution to the next frame
            }

            // After the loop, elapsedTime will be approximately equal to timeDuration
            UIController.Instance.DisplayPopUpMessage(_navigationContent);
        }

        public void SwitchView()
        {
            isTPS = !isTPS;

            _tpsCamera.SetActive(isTPS);
            _tpsVirtualCamera.SetActive(isTPS);
            _fpsCamera.SetActive(!isTPS);
        }

        public void StartSpawnController()
        {
            _spawnController.StartNextWave();
        }

        public void SetGameOver()
        {
            CurrentGameState = GameState.GameOver;
            OnGameStateChanged?.Invoke(GameState.GameOver);
            UIController.Instance.GameOver();
        }

        public void SetPlayAgain()
        {
            CurrentGameState = GameState.Playing;
            UIController.Instance.PlayAgain();
        }
        #endregion

    }
}