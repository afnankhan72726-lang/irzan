using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public sealed class WeaponHitbox : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private BoxCollider _hitboxCollider;
    [SerializeField] private Animator _playerAnimator;

    [Header("Distinct Attack Damage Values")]
    [SerializeField] private int _combo1Damage = 20;
    [SerializeField] private int _combo2Damage = 30;
    [SerializeField] private int _heavyAttackDamage = 50;
    [SerializeField] private int _defaultDamage = 15;

    private readonly HashSet<Collider> _hitTargets = new();

    private void Awake()
    {
        if (_hitboxCollider == null) _hitboxCollider = GetComponent<BoxCollider>();
        if (_playerAnimator == null) _playerAnimator = GetComponentInParent<Animator>();

        _hitboxCollider.isTrigger = true;
        _hitboxCollider.enabled = false;
    }

    public void BeginAttackHit()
    {
        _hitTargets.Clear();
        _hitboxCollider.enabled = true;
    }

    public void EndAttackHit()
    {
        _hitboxCollider.enabled = false;
        _hitTargets.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_hitboxCollider.enabled) return;

        // Prevent striking ourselves if the blade hits our own player capsule
        if (other.transform.root == transform.root) return;

        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth == null) targetHealth = other.GetComponentInParent<Health>();

        if (targetHealth == null) return;
        if (!_hitTargets.Add(other)) return;

        int calculatedDamage = DetermineCurrentAttackDamage();
        targetHealth.TakeDamage(calculatedDamage);
    }

    private int DetermineCurrentAttackDamage()
    {
        if (_playerAnimator == null) return _defaultDamage;

        // Checks layer 1 (CombatLayer) in your Animator
        AnimatorStateInfo stateInfo = _playerAnimator.GetCurrentAnimatorStateInfo(1);

        if (stateInfo.IsTag("Combo1")) return _combo1Damage;
        if (stateInfo.IsTag("Combo2")) return _combo2Damage;
        if (stateInfo.IsTag("Heavy")) return _heavyAttackDamage;

        return _defaultDamage;
    }

    private void OnDisable()
    {
        if (_hitboxCollider != null) _hitboxCollider.enabled = false;
        _hitTargets.Clear();
    }
}
