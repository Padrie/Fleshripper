using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject eventSystem;

    bool toggle = false;

    public static bool isIngame = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(eventSystem);
    }

    private void OnEnable()
    {
        Player.pressedEsc += ShowPauseMenu;
        Player.onPlayerDied += SetBoolToFalse;
        StatusScreen.onGameReset += SetBoolToTrue;
        LevelReset.onLevelFinished += SetBoolToFalse;
    }

    private void OnDisable()
    {
        Player.pressedEsc -= ShowPauseMenu;
        Player.onPlayerDied -= SetBoolToFalse;
        StatusScreen.onGameReset -= SetBoolToTrue;
        LevelReset.onLevelFinished -= SetBoolToFalse;
    }

    public void SetBoolToFalse()
    {
        isIngame = false;
    }

    public void SetBoolToTrue()
    {
        isIngame = true;
    }

    public void SetIngame(bool toggle)
    {
        isIngame = toggle;
    }

    public void OpenOptionsMenu()
    {
        UIManager.OptionsMenu();
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
        TutorialManager.machineGunUnlocked = false;
        TutorialManager.shotgunUnlocked = false;
        TutorialManager.explosionPickupUnlocked = false;
        TutorialManager.lightningPickupUnlocked = false;
    }

    public void CloseGame()
    {
        isIngame = false;
        ShowCursor();
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
        TutorialManager.machineGunUnlocked = false;
        TutorialManager.shotgunUnlocked = false;
        TutorialManager.explosionPickupUnlocked = false;
        TutorialManager.lightningPickupUnlocked = false;
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

    public void ShowPauseMenu()
    {
        toggle = !toggle;
        isIngame = true;
        pauseMenu.SetActive(toggle);
    }

    private void Update()
    {
        if (isIngame)
        {
            if (pauseMenu.activeSelf)
            {
                SetTimeScale(0f);
                toggle = true;
                ShowCursor();
            }
            else
            {
                SetTimeScale(1f);
                toggle = false;
                HideCursor();
            }
        }
    }
}
