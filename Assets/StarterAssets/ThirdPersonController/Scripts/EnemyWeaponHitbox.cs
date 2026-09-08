using UnityEngine;

public sealed class EnemyWeaponHitbox : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private int _damageToPlayer = 15;
    [SerializeField] private Collider _weaponCollider;

    private void Awake()
    {
        if (_weaponCollider == null) _weaponCollider = GetComponent<Collider>();

        // Safety: ensure it handles intersection overlays cleanly
        if (_weaponCollider != null) _weaponCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the Orc's physical collider touched our player character framework
        if (other.CompareTag("Player") || other.gameObject.name.Contains("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth == null) playerHealth = other.GetComponentInParent<Health>();

            if (playerHealth != null)
            {
                // Deliver the raw operational impact damage!
                playerHealth.TakeDamage(_damageToPlayer);
                Debug.Log($"👹 Ork Destroyer struck Player for {_damageToPlayer} damage!");
            }
        }
    }
}
