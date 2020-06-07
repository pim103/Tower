using UnityEngine;
using UnityEngine.EventSystems;
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
            createButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.Connection);
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
        }

        public void InitMenu()
        {
            Debug.Log("Registration Menu");
            //throw new System.NotImplementedException();
        }
    }
}