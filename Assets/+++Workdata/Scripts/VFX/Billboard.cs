using UnityEngine;

public class Billboard : MonoBehaviour
{
    Player player;

    public bool lockX = false;
    public bool lockY = false;
    public bool lockZ = false;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        HandleRotation();
    }

    public void HandleRotation()
    {
        Vector3 direction = player.transform.position - transform.position;

        if (lockX) direction.x = 0f;
        if (lockY) direction.y = 0f;
        if (lockZ) direction.z = 0f;

        if (direction == Vector3.zero) return;

        direction.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }
}
