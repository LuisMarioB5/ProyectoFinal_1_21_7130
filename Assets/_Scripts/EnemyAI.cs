using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int maxHealth = 3;
    public int scoreValue = 10;

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
    }

    void Update()
    {
        if (isDead || player == null) return;

        agent.SetDestination(player.position);

        if (agent.velocity.magnitude > 0.1f) animator.SetBool("isRunning", true);
        else animator.SetBool("isRunning", false);
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
        animator.SetTrigger("Die");

        Destroy(gameObject, 3f);
    }
}
