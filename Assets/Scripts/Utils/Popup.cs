using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//Author : Attika

namespace Utils
{
    public class Popup : MonoBehaviour
    {
        private static Popup _this;

        [SerializeField] private GameObject background;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI description;
        private bool callbackAdded;

        //Attribution (instance of this popup) and initialization (add Close function to closeButton and set background to false)  
        private void Awake()
        {
            if(_this == null)
                _this = this;

            _this.callbackAdded = false;
            _this.closeButton.onClick.AddListener(Close);
            background.SetActive(false);
        }

        #region customize attributes methods

        //Customize title
        public void SetTitle(string value)
        {
            _this.title.text = value;
        }

        //Customize description
        public void SetDescription(string value)
        {
            _this.description.text = value;
        }

        //Add a callback to closeButton
        public void AddListenerToCloseButton(UnityAction callback)
        {
            _this.closeButton.onClick.AddListener(callback);
            if(!_this.callbackAdded)
                _this.callbackAdded = true;
        }
        
        #endregion
        
        #region activation and deactivation methods
        
        //Open popup
        public static void Open()
        {
            _this.background.SetActive(true);
        }

        //Close popup, and if there is other(s) callback(s), clear them
        private static void Close()
        {
            _this.background.SetActive(false);
            if (_this.callbackAdded)
            {
                ReinitializeCallbacks();
            }
        }

        //Clear all callbacks from closeButton and reinitialize with Close function
        private static void ReinitializeCallbacks()
        {
            _this.closeButton.onClick.RemoveAllListeners();
            _this.closeButton.onClick.AddListener(Close);
            _this.callbackAdded = false;
        }
        
        #endregion
    }
}
