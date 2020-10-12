using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

//Author : Attika

namespace Utils
{
    //TODO : une fois fini de dev le systeme de popup, supprimer les commentaires dans PopupOptions (ou les réduire au strict minimum)
    [System.Serializable]
    public class PopupOptions
    {
        public bool isContextualDialog; // La popup est prioritaire sur tous les autres ui, et on bloque les input qui ne la concerne pas tant qu'elle n'est pas fermée (en effectuant une certaine action => oui/non, fermer la popup... etc)
        public bool isDynamicPosition; // La popup doit se placer dynamiquement à l'écran par rapport à une autre ui (droite/gauche, haut/bas) selon la place dispo
        public bool isDynamicSize; // La popup doit être resize selon le contenu (qui n'est pas déterminé à l'avance) => toutes les popup ne devraient-elles pas avoir cette règle ? 
        public bool isAQuestion; // La popup doit contenir deux boutons (voire trois) à laquelle une action doit être liée 
        public bool isAnimated; // La popup a une petite animation d'apparition (et de disparition ?)
        //TODO : d'autres choses à ajouter ?    
    }

    public class Popup : MonoBehaviour
    {
        private static Popup _this;

        [Header("General Parameters")]
        [SerializeField] private GameObject background;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI description;
        private bool callbackAdded;
        [SerializeField] private PopupOptions options;
        
        [Header("Contextual Parameters")]
        [SerializeField] private GameObject contextualUiBlock;
        
        [Header("Question Parameters")]
        [SerializeField] private GameObject question;
        [SerializeField] private TextMeshProUGUI titleQuestion;
        [SerializeField] private TextMeshProUGUI validateButtonText;
        [SerializeField] private Button validateButton;
        private bool validateCallbackAdded;
        [SerializeField] private TextMeshProUGUI cancelButtonText;
        [SerializeField] private Button cancelButton;
        private bool cancelCallbackAdded;
        [SerializeField] private TextMeshProUGUI thirdButtonText;
        [SerializeField] private Button thirdButton;
        private bool thirdCallbackAdded;

        [Header("Animation parameters")]
        [SerializeField] private Animator animator;
        
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

        
        /* General customization */
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

        /* Question customization */
        //Customize question title
        public void SetTitleQuestion(string value)
        {
            if (options.isAQuestion)
                Debug.LogWarning(
                    "Trying to customize popup's question attributes but the question option is not checked on this popup");
            else
                titleQuestion.text = value;
        }

        //Customize validate text
        public void SetValidateButtonText(string value)
        {
            if (options.isAQuestion)
                Debug.LogWarning(
                    "Trying to customize popup's question attributes but the question option is not checked on this popup");
            else
                validateButtonText.text = value;
        }
        
        //Customize cancel text
        public void SetCancelButtonText(string value)
        {
            if (options.isAQuestion)
                Debug.LogWarning(
                    "Trying to customize popup's question attributes but the question option is not checked on this popup");
            else
                cancelButtonText.text = value;
        }
        
        //Customize third button text
        public void SetThirdButtonText(string value)
        {
            if (options.isAQuestion)
                Debug.LogWarning(
                    "Trying to customize popup's question attributes but the question option is not checked on this popup");
            else
                thirdButtonText.text = value;
        }
        
        //Add a callback on validate button
        public void AddListenerToValidateButton(UnityAction callback)
        {
            _this.validateButton.onClick.AddListener(callback);
            if(!_this.validateCallbackAdded)
                _this.validateCallbackAdded = true;
        }
        
        //Add a callback on cancel button
        public void AddListenerToCancelButton(UnityAction callback)
        {
            _this.cancelButton.onClick.AddListener(callback);
            if(!_this.cancelCallbackAdded)
                _this.cancelCallbackAdded = true;
        }
        
        //Add a callback on third button
        public void AddListenerToThirdButton(UnityAction callback)
        {
            _this.thirdButton.onClick.AddListener(callback);
            if (!_this.thirdCallbackAdded)
                _this.thirdCallbackAdded = true;
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
