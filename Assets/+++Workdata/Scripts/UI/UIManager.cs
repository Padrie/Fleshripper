using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static event Action openOptionsMenu;
    static bool scenesAlreadyLoaded = false;

    private void Start()
    {
        if (!scenesAlreadyLoaded)
        {
            SceneManager.LoadScene("OptionsScroll", LoadSceneMode.Additive);
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
            scenesAlreadyLoaded = true;
        }
    }

    public static void OptionsMenu()
    {
        openOptionsMenu?.Invoke();
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void StartGame()
    {
        PauseMenu.isIngame = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
