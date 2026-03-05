using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public GameObject options;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        Player.pressedEsc += CloseOptionsMenu;
        UIManager.openOptionsMenu += OpenOptionsMenu;
    }

    private void OnDisable()
    {
        Player.pressedEsc += CloseOptionsMenu;
        UIManager.openOptionsMenu -= OpenOptionsMenu;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseOptionsMenu();
        }
    }

    public void OpenOptionsMenu()
    {
        options.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        options.SetActive(false);
    }

    public void Fullscreen(bool toggle)
    {
        Screen.fullScreen = toggle;
    }

    public void Brightness(float value)
    {
        UnityEngine.Rendering.VolumeProfile volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        // You can leave this variable out of your function, so you can reuse it throughout your class.
        UnityEngine.Rendering.Universal.Vignette vignette;

        if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));

        vignette.intensity.Override(0.5f);
    }
}
