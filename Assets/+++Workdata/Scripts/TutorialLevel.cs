using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialLevel : MonoBehaviour
{
    [SerializeField] GameObject gateToHeaven;
    [SerializeField] bool open = false;
    [SerializeField] bool close = false;

    private void Start()
    {
        if (gateToHeaven != null)
            gateToHeaven.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (open)
                gateToHeaven.SetActive(true);
            if (close)
                gateToHeaven.SetActive(false);
        }
    }
}
