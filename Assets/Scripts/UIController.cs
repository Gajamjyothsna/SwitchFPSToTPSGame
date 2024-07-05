using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TPSGame
{
    public class UIController : MonoBehaviour
    {
        #region Creating Instance
        private static UIController _uiControllerInstance;

        public static UIController Instance
        {
            get
            {
                if (_uiControllerInstance == null)
                {
                    _uiControllerInstance = FindObjectOfType<UIController>();
                }
                return _uiControllerInstance;
            }
        }
        #endregion

        #region Private Variables
        [Header("UI Components")]
        [SerializeField] private GameObject _popUpMessage;
        [SerializeField] private TextMeshProUGUI _popUpTextTMP;
        [SerializeField] private TextMeshProUGUI _keyTMP;
        [Header("Health Values")]
        [SerializeField] private int _maxPlayerHealth = 100;
        [Header("Health UI Components")]
        [SerializeField] private Slider _playerHealthSlider;
        [Header("GameOver UI")]
        [SerializeField] private GameObject _gameOverUI;
        [SerializeField] private GameObject _gamePanelUI;
        private int _currentPlayerHealth;
        public string key;
        #endregion

        #region MonoBehaviour Methods
        private void Awake()
        {
            if (_uiControllerInstance == null)
            {
                _uiControllerInstance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_uiControllerInstance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (TPSGameManager.Instance.CurrentGameState == TPSGameDataModel.GameState.GameOver) return;
            _currentPlayerHealth = _maxPlayerHealth;
            DisplayPlayerHealth();
        }
        #endregion

        #region private Methods
        private void DisplayPlayerHealth()
        {
            Debug.Log("CurrentHealth :" + _currentPlayerHealth);
            Debug.Log("Max Health" + _maxPlayerHealth);
            _playerHealthSlider.value = (float)_currentPlayerHealth / _maxPlayerHealth;
        }
        #endregion

        #region Public Methods
        public void DisplayPopUpMessage(string message)
        {
            _popUpMessage.SetActive(true);
           _popUpTextTMP.text = message;

            Invoke("HidePopUpMessage", 5f);
        }

        public void DisplayKey(string _key)
        {
            _keyTMP.text += _key;
            key += _key;
        }
        #endregion

        #region Private Methods
        private void HidePopUpMessage()
        {
            _popUpMessage.SetActive(false);
        }

        public void UpdatePlayerHealth(int damage, bool isIncrease)
        {
            Debug.Log("UpdatePlayerHealth");
            if (TPSGameManager.Instance.CurrentGameState == TPSGameDataModel.GameState.GameOver) return;
            if (!isIncrease)
            {
                Debug.Log("False");
                _currentPlayerHealth -= damage;
                if (_currentPlayerHealth < 0)
                {
                    _currentPlayerHealth = 0; // Ensure health doesn't go below 0
                }
            }
            else
            {
                Debug.Log("True");
                _currentPlayerHealth += damage;
                if (_currentPlayerHealth > _maxPlayerHealth)
                {
                    _currentPlayerHealth = _maxPlayerHealth; // Ensure health doesn't exceed max health
                }
            }

            DisplayPlayerHealth();

           if (_currentPlayerHealth == 0) TPSGameManager.Instance.SetGameOver();
        }

        public void GameOver()
        {
            _gameOverUI.SetActive(true);
            _gamePanelUI.SetActive(false );
        }

        public void PlayAgain()
        {
            _gameOverUI.SetActive(false);
            _gamePanelUI.SetActive(true);
        }

        #endregion


    }
}
