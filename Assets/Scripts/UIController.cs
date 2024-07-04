using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        #endregion


    }
}
