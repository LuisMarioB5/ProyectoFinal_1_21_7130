using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int maxHealth = 3;
    public int scoreValue = 10;
    public float attackRange = 2f;

    private int currentHealth;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        agent.stoppingDistance = attackRange - 0.5f;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange) Attack();
        else ChasePlayer();
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        animator.SetBool("isAttacking", false);
        animator.SetBool("isRunning", true);
    }

    void Attack()
    {
        agent.isStopped = true;

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", true);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        GetComponent<Collider>().enabled = false;

        transform.Rotate(-90, 0, 0);

        Destroy(gameObject, 1.5f);
    }
}
