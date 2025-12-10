using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int maxHealth = 3;
    public int damage = 20;
    public float attackCooldown = 1.5f;
    public float attackRange = 2f;
    public int scoreValue = 10;

    private int currentHealth;
    private float nextAttackTime = 0f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.stoppingDistance = attackRange - 0.5f;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
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

        HandleAnimations(isAttacking: false, isRunning: true);
    }

    void Attack()
    {
        agent.isStopped = true;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        HandleAnimations(isRunning: false, isAttacking: true);

        if (Time.time >= nextAttackTime)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null) playerHealth.TakeDamage(damage);
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        GetComponent<Collider>().enabled = false;

        HandleAnimations();

        transform.Rotate(-90, 0, 0);
        Destroy(gameObject, 1.5f);
    }

    void HandleAnimations(bool isRunning = false, bool isAttacking = false)
    {
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isAttacking", isAttacking);
    }
}
