using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//Author : Attika

namespace Utils
{
    //TODO : une fois fini de dev le systeme de popup, supprimer les commentaires dans PopupOptions (ou les réduire au strict minimum)
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

        [SerializeField] private GameObject background;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI description;
        private bool callbackAdded;
        private PopupOptions options;

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
