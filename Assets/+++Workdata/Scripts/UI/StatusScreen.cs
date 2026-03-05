using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatusScreen : MonoBehaviour
{
    public GameObject nextLevelScreen;
    public GameObject deathScreen;

    public static event Action onGameReset;

    void Update()
    {
        /*
        if (Time.timeScale != 0f)
            Debug.Log("Time.timeScale changed back to " + Time.timeScale);

        if (Cursor.visible == false || Cursor.lockState != CursorLockMode.None)
            Debug.Log("Cursor changed back: visible=" + Cursor.visible + ", lockState=" + Cursor.lockState);
        */
    }

    private void OnEnable()
    {
        Player.onPlayerDied += ShowDeathScreen;
        LevelReset.onLevelFinished += ShowNextLevelScreen;
    }

    private void OnDisable()
    {
        Player.onPlayerDied -= ShowDeathScreen;
        LevelReset.onLevelFinished -= ShowNextLevelScreen;
    }

    public void ShowNextLevelScreen()
    {
        ShowCursor();
        SetGameSpeed(0f);
        nextLevelScreen.SetActive(true);
    }

    public void ShowDeathScreen()
    {
        Debug.Log("ShowDeathScreen called");

        ShowCursor();
        SetGameSpeed(0f);

        Debug.Log("Cursor.visible: " + Cursor.visible);
        Debug.Log("Cursor.lockState: " + Cursor.lockState);
        Debug.Log("Time.timeScale: " + Time.timeScale);

        deathScreen.SetActive(true);
    }

    public void ResetLevel()
    {
        if (SceneManager.GetActiveScene().name == "NaomiTestScene")
        {
            SetGameSpeed(1f);
            HideCursor();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            onGameReset?.Invoke();

            TutorialManager.machineGunUnlocked = false;
            TutorialManager.shotgunUnlocked = false;
            TutorialManager.explosionPickupUnlocked = false;
            TutorialManager.lightningPickupUnlocked = false;
        }
        else if (SceneManager.GetActiveScene().name == "TufanPrototype LvL")
        {
            SetGameSpeed(1f);
            HideCursor();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            onGameReset?.Invoke();

            TutorialManager.machineGunUnlocked = true;
            TutorialManager.shotgunUnlocked = true;
            TutorialManager.explosionPickupUnlocked = false;
            TutorialManager.lightningPickupUnlocked = false;
        }
        else if (SceneManager.GetActiveScene().name == "Tufan LVL 2")
        {
            SetGameSpeed(1f);
            ShowCursor();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            onGameReset?.Invoke();

            TutorialManager.machineGunUnlocked = true;
            TutorialManager.shotgunUnlocked = true;
            TutorialManager.explosionPickupUnlocked = true;
            TutorialManager.lightningPickupUnlocked = true;
        }
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().name == "NaomiTestScene")
        {
            SetGameSpeed(1f);
            HideCursor();
            SceneManager.LoadScene(2);
        }
        else if (SceneManager.GetActiveScene().name == "TufanPrototype LvL")
        {
            SetGameSpeed(1f);
            HideCursor();
            SceneManager.LoadScene(3);
        }
        else if (SceneManager.GetActiveScene().name == "Tufan LVL 2")
        {
            SetGameSpeed(1f);
            ShowCursor();
            SceneManager.LoadScene(0);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        TutorialManager.machineGunUnlocked = false;
        TutorialManager.shotgunUnlocked = false;
        TutorialManager.explosionPickupUnlocked = false;
        TutorialManager.lightningPickupUnlocked = false;
    }

    public void SetGameSpeed(float speed)
    {
        Time.timeScale = speed;
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
