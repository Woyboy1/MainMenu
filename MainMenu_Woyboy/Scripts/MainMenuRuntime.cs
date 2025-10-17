using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMenu_Woyboy
{
    public class MainMenuRuntime : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject settingsMenu;

        [Header("Input")]
        [SerializeField] private bool useInput = true; [Tooltip("Allow the script to " +
            "check for input to open main menu.")]
        [SerializeField] private KeyCode openMainMenuKey = KeyCode.Escape;

        private bool isOpen = false;

        private void Start()
        {
            CloseMenu();
        }

        private void Update()
        {
            GetInput();
        }

        private void GetInput()
        {
            if (useInput && Input.GetKeyDown(openMainMenuKey))
            {
                ToggleMenus(!isOpen);
            }
        }

        private void ToggleMenus(bool open)
        {
            isOpen = open;

            if (isOpen)
            {
                OpenMainMenu();
            }
            else
            {
                CloseMenu();
            }
        }

        private void ToggleCursor(bool toggle)
        {
            Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = toggle;
        }

        public void OpenMainMenu()
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
            ToggleCursor(true);
        }

        public void OpenSettingsMenu()
        {
            settingsMenu.SetActive(true);
            mainMenu.SetActive(false);
            ToggleCursor(true);
        }

        public void CloseMenu()
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(false);
            ToggleCursor(false);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}