using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manages navigation through the menu system
public static class MenuManager {
    // Goes to the menu with the given name
    public static void GoToMenu(MenuName name) {
        switch (name) {
            case MenuName.Gameplay:
                // go to gameplay scene
                SceneManager.LoadScene("02_GamePlay");
                break;
            case MenuName.MainMenu:
                // go to MainMenu scene
                SceneManager.LoadScene("01_MainMenu");
                break;
            case MenuName.Pause:
                // instantiate prefab
                Object.Instantiate(Resources.Load("PauseMenu"));
                break;
        }
    }
}