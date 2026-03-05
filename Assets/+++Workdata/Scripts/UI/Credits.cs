using UnityEngine;

public class Credits : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            gameObject.SetActive(false);
    }

    public void OpenURL(string url)
    {
        if (url == "")
        {
            print("No URL");
        }
        else
        {
            Application.OpenURL(url);
        }
    }
}
