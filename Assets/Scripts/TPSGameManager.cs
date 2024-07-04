using System.Collections;
using UnityEngine;

namespace TPSGame
{
    public class TPSGameManager : MonoBehaviour
    {
        #region GameType Model
        public enum GameState
        {
            Playing,
            GameOver
        }
        #endregion

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
        public GameState CurrentGameState { get; private set; } = GameState.Playing;
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
        #endregion

    }
}