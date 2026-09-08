using UnityEngine;

public sealed class MutantAI : MonoBehaviour
{
    [Header("Tracking Parameters")]
    [SerializeField] private Transform _playerTarget;
    [SerializeField] private float _chaseSpeed = 3.0f;
    [SerializeField] private float _attackRange = 2.2f;

    [Header("Combat Pacing")]
    [SerializeField] private float _attackCooldown = 2.0f;
    private float _nextAttackTime;

    [Header("Dependencies")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider _weaponHitbox;

    private void Start()
    {
        if (_playerTarget == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) player = GameObject.Find("PlayerArmature");
            if (player != null) _playerTarget = player.transform;
        }

        if (_animator == null) _animator = GetComponent<Animator>();

        DisableEnemyDamage();
    }

    private void Update()
    {
        if (_playerTarget == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _playerTarget.position);

        if (distanceToPlayer > _attackRange)
        {
            ChasePlayer();
        }
        else
        {
            TryAttackPlayer();
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (_playerTarget.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 8f);
        }

        transform.position += transform.forward * _chaseSpeed * Time.deltaTime;

        // Bypassed Speed Parameter: Force the animator to blend back into locomotion manually if needed
    }

    private void TryAttackPlayer()
    {
        if (Time.time >= _nextAttackTime)
        {
            ExecuteEnemySwing();
            _nextAttackTime = Time.time + _attackCooldown;
        }
    }

    private void ExecuteEnemySwing()
    {
        if (_animator != null)
        {
            // Generates a random number: 1, 2, or 3
            int randomAttackIndex = Random.Range(1, 4);

            // Combines the word with the number matching his exact spelling: "atack1", "atack2", "atack3"
            string targetAttackState = "atack" + randomAttackIndex;

            // Forcefully play the random attack state directly!
            _animator.Play(targetAttackState, 0, 0f);

            Debug.Log($"👹 Ork Destroyer rolled a random move and triggered: {targetAttackState}");
        }
        else
        {
            BeginEnemyDamage();
            EndEnemyDamage();
        }
    }


    // --- ANIMATION TIMING EVENT RECEIVERS ---
    public void BeginEnemyDamage()
    {
        if (_weaponHitbox != null) _weaponHitbox.enabled = true;
    }

    public void EndEnemyDamage()
    {
        if (_weaponHitbox != null) _weaponHitbox.enabled = false;
    }

    public void DisableEnemyDamage()
    {
        if (_weaponHitbox != null) _weaponHitbox.enabled = false;
    }
}
