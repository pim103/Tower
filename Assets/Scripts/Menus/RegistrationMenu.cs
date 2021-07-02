using System;
using System.Collections;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Menus
{
    public class RegistrationMenu : MonoBehaviour, MenuInterface
    {
        [SerializeField]
        private MenuController mc;

        [SerializeField]
        private InputField loginField;

        [SerializeField]
        private InputField passwordField;

        [SerializeField]
        private InputField confirmPasswordField;

        [SerializeField]
        private InputField emailField;

        [SerializeField]
        private Button createButton;

        [SerializeField]
        private Button returnButton;

        private void Start()
        {
            passwordField.inputType = InputField.InputType.Password;
            confirmPasswordField.inputType = InputField.InputType.Password;
            createButton.onClick.AddListener(delegate
            {
                CallRegister();
            });

            returnButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.Connection);
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (EventSystem.current.currentSelectedGameObject != null)
                    {
                        Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                        if (selectable != null)
                            selectable.Select();
                    }
                }
                else
                {
                    if (EventSystem.current.currentSelectedGameObject != null)
                    {
                        Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                        if (selectable != null)
                            selectable.Select();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                CallRegister();
            }
        }

        public void InitMenu()
        {
            Debug.Log("Registration Menu");
        }

        public void CallRegister()
        {
            StartCoroutine(Register());
        }

        IEnumerator Register()
        {
            WWWForm form = new WWWForm();
            form.AddField("gameToken", NetworkingController.GameToken);
            form.AddField("accountName", loginField.text);
            form.AddField("accountEmail", emailField.text);
            form.AddField("accountPassword", passwordField.text);
            form.AddField("accountPasswordConfirmation", confirmPasswordField.text);
            var www = UnityWebRequest.Post(NetworkingController.PublicURL + "/services/account/add.php", form);
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
                //DisplayErrors(www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
                if (www.responseCode == 201)
                {
                    mc.ActivateMenu(MenuController.Menu.Connection);
                }
            }
        }
    }
}