using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Listens for the OnClick events for the main menu buttons
public class MainMenu : MonoBehaviour
{
    // Handles the on click event from the play button
    public void HandlePlayButtonOnClickEvent() {
        MenuManager.GoToMenu(MenuName.Gameplay);
    }

    // Handles the on click event from the quit button
    public void HandleQuitButtonOnClickEvent() {
        Application.Quit();
    }
}
