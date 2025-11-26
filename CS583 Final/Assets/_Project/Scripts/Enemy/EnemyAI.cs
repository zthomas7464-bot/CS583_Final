using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;         // Assign in inspector or found in Start

    private CharacterController controller;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float wanderSpeed = 1.5f;
    public float gravity = -9.81f;

    [Header("Vision")]
    public float viewDistance = 10f; // How far the enemy can see
    [Range(0f, 360f)]
    public float viewAngle = 90f;    // Field of view in degrees

    [Header("Attack")]
    public int damage = 1;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    // internal
    private float verticalVelocity;
    private Vector3 wanderDirection;
    private float wanderTimer = 0f;
    public float wanderDirectionChangeInterval = 3f;

    private float lastAttackTime = -999f;

    // NEW: remembers if we are in chase mode
    private bool isChasing = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // If player not set in inspector, find by tag
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Gravity
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;
        else
            verticalVelocity += gravity * Time.deltaTime;

        Vector3 horizontalMove = Vector3.zero;

        // ─── VISION STATE LOGIC ────────────────────────────────
        bool canSeeNow = CanSeePlayerInFOV();     // distance + FOV + LOS
        bool hasLOS = HasLineOfSightToPlayer();   // distance + LOS only

        // if we see the player in our cone → start chasing
        if (canSeeNow)
            isChasing = true;

        // if we are chasing but lose line of sight (wall between us) → stop chasing
        if (isChasing && !hasLOS)
            isChasing = false;

        // ─── MOVEMENT ──────────────────────────────────────────
        if (isChasing)
        {
            // CHASE
            Vector3 dirToPlayer = (player.position - transform.position);
            dirToPlayer.y = 0f;
            dirToPlayer.Normalize();

            horizontalMove = dirToPlayer * moveSpeed;

            // smooth rotate toward player
            if (dirToPlayer.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dirToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime);
            }

            TryAttack();
        }
        else
        {
            // WANDER
            UpdateWanderDirection();
            horizontalMove = wanderDirection * wanderSpeed;

            if (wanderDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(wanderDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 3f * Time.deltaTime);
            }
        }

        Vector3 move = horizontalMove;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }

    // ──────────────────────────────────────
    // Vision helpers
    // ──────────────────────────────────────

    // Used to START the chase: needs FOV + distance + LOS
    bool CanSeePlayerInFOV()
    {
        if (player == null) return false;

        Vector3 toPlayer = player.position - transform.position;
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer > viewDistance) return false;

        // check angle on XZ plane
        Vector3 toPlayerXZ = new Vector3(toPlayer.x, 0f, toPlayer.z).normalized;
        float angle = Vector3.Angle(transform.forward, toPlayerXZ);
        if (angle > viewAngle * 0.5f) return false;

        // must ALSO have clear line of sight
        return HasLineOfSightToPlayer();
    }

    // Used while chasing: only care about distance + LOS, not angle
    bool HasLineOfSightToPlayer()
    {
        if (player == null) return false;

        Vector3 origin = transform.position + Vector3.up * 1f;
        Vector3 target = player.position + Vector3.up * 1f;
        Vector3 dir = target - origin;
        float distance = dir.magnitude;

        if (distance > viewDistance) return false;

        dir.Normalize();

        RaycastHit hit;
        // No LayerMask here → hits everything; first hit must be the player
        if (Physics.Raycast(origin, dir, out hit, distance))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }

    // ──────────────────────────────────────
    // Wandering
    // ──────────────────────────────────────
    void UpdateWanderDirection()
    {
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0f || wanderDirection == Vector3.zero)
        {
            wanderTimer = wanderDirectionChangeInterval;

            // pick random direction on XZ plane
            Vector2 rand = Random.insideUnitCircle.normalized;
            wanderDirection = new Vector3(rand.x, 0f, rand.y);
        }
    }

    // ──────────────────────────────────────
    // Attacking / Damage
    // ──────────────────────────────────────
    void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
                lastAttackTime = Time.time;
            }
        }
    }

    // ──────────────────────────────────────
    // GIZMOS (Scene View Only)
    // ──────────────────────────────────────
    void OnDrawGizmos()
    {
        // Draw view radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        // Draw viewing angle lines
        Vector3 origin = transform.position;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin, origin + leftBoundary * viewDistance);
        Gizmos.DrawLine(origin, origin + rightBoundary * viewDistance);

        // Draw forward direction
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(origin, origin + transform.forward * viewDistance);

        // Draw LOS line when chasing
        if (player != null)
        {
            Gizmos.color = isChasing ? Color.red : Color.green;
            Gizmos.DrawLine(transform.position + Vector3.up * 1f,
                            player.position + Vector3.up * 1f);
        }
    }
}
