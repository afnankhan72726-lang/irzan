using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(StarterAssetsInputs))]
public sealed class PlayerCombat : MonoBehaviour
{
    // Performance optimization: cache parameter hashes into memory strings
    private static readonly int Attack1Hash = Animator.StringToHash("Attack1");
    private static readonly int Attack2Hash = Animator.StringToHash("Attack2");
    private static readonly int Attack3Hash = Animator.StringToHash("Attack3");

    [Header("Dependencies")]
    [SerializeField] private Animator _animator;
    [SerializeField] private StarterAssetsInputs _input;

    [Header("Hitbox Link")]
    [SerializeField] private WeaponHitbox _weaponHitbox;

    private void Awake()
    {
        if (_animator == null) _animator = GetComponent<Animator>();
        if (_input == null) _input = GetComponent<StarterAssetsInputs>();
        if (_weaponHitbox == null) _weaponHitbox = GetComponentInChildren<WeaponHitbox>();
    }

    private void Update()
    {
        HandleCombatHotkeys();
    }

    private void HandleCombatHotkeys()
    {
        // Hotkey 1: Basic Left Click / F
        if (_input.attack1)
        {
            _animator.SetTrigger(Attack1Hash);
            _input.attack1 = false;
            return;
        }

        // Hotkey 2: Medium Right Click
        if (_input.attack2)
        {
            _animator.SetTrigger(Attack2Hash);
            _input.attack2 = false;
            return;
        }

        // Hotkey 3: Heavy E Key Strike
        if (_input.attack3)
        {
            _animator.SetTrigger(Attack3Hash);
            _input.attack3 = false;
            return;
        }
    }

    // --- ANIMATION EVENT ROUTERS ---
    public void BeginAttackHit()
    {
        if (_weaponHitbox != null) _weaponHitbox.BeginAttackHit();
    }

    public void EndAttackHit()
    {
        if (_weaponHitbox != null) _weaponHitbox.EndAttackHit();
    }
}
