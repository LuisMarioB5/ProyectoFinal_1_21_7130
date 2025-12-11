using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int maxHealth = 3;
    public int damage = 20;
    public float attackCooldown = 1.5f;
    public float attackRange = 2f;
    public int scoreValue = 10;
    public float hitDelay = 0.3f;

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
        
        if (agent!= null) agent.stoppingDistance = attackRange - 0.5f;

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

        animator.SetBool("isRunning", true);
    }

    void Attack()
    {
        agent.isStopped = true;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        animator.SetBool("isRunning", false);

        if (Time.time >= nextAttackTime)
        {
            animator.SetTrigger("AttackTrigger");
            
            StartCoroutine(DoDamageDelayed());
            
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    IEnumerator DoDamageDelayed()
    {
        yield return new WaitForSeconds(hitDelay);

        if (!isDead && player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange + 1f)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null) playerHealth.TakeDamage(damage);
            }
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

        animator.SetBool("isRunning", false);

        transform.Rotate(-90, 0, 0);

        if (GameManager.instance != null) GameManager.instance.UpdateEnemiesCounter(-1);

        Destroy(gameObject, 1.5f);
    }
}
