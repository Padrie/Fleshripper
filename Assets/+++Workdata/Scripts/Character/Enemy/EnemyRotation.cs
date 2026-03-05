using UnityEngine;
using UnityEngine.AI;

public class EnemyRotation : MonoBehaviour
{
    private Enemy enemy;
    private Animator animator;
    private Player player;

    public bool isDead = false;
    public bool noEnemyScript = false;

    private NavMeshAgent noEnemyNavMeshAgent;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        noEnemyNavMeshAgent = GetComponentInParent<NavMeshAgent>();
    }

    private void Start()
    {
        animator.Play("Walk");
        animator.SetFloat("Offset", Random.Range(0f, 1f));
    }

    private void Update()
    {
        if (isDead)
            HandleDeadOrientation();
        else
            HandleOrientation();
    }

    private void HandleOrientation()
    {
        Vector3 worldVelocity = enemy.agent.velocity;

        Vector3 localDir = enemy.transform.InverseTransformDirection(worldVelocity.normalized);

        animator.SetFloat("X", Mathf.Clamp(localDir.x, -1f, 1f));
        animator.SetFloat("Y", Mathf.Clamp(localDir.z, -1f, 1f));
    }

    public void HandleDeadOrientation()
    {
        if (noEnemyScript)
        {
            print(-DirectionTo(player.transform.position).x);
            animator.SetFloat("X", -DirectionTo(player.transform.position).x);
            animator.SetFloat("Y", -DirectionTo(player.transform.position).z);
        }
        else
        {
            animator.SetFloat("X", -enemy.DirectionTo(player.transform.position).x);
            animator.SetFloat("Y", -enemy.DirectionTo(player.transform.position).z);
        }
    }

    public Vector3 DirectionTo(Vector3 position)
    {
        return new Vector3(position.x - transform.position.x, position.y - transform.position.y,
            position.z - transform.position.z).normalized;
    }

    public void DisableCollider()
    {
        Collider[] col = GetComponentsInParent<Collider>();

        for (int i = 0; i < col.Length; i++)
        {
            col[i].enabled = false;
        }
    }
}
