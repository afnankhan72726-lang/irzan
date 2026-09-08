using UnityEngine;
using UnityEngine.Events;
using TMPro; // Crucial: Gives access to TextMeshPro code tools

public sealed class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;

    [Header("UI Text Dependency (Optional)")]
    [SerializeField] private TextMeshProUGUI _healthTextDisplay;

    [Header("Events")]
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent OnDeath;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        UpdateTextUI();
    }

    public void TakeDamage(int damageAmount)
    {
        if (_currentHealth <= 0) return;

        _currentHealth -= damageAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        // Update Slider UI
        float healthNormalized = (float)_currentHealth / _maxHealth;
        OnHealthChanged?.Invoke(healthNormalized);

        // Update Text UI
        UpdateTextUI();

        Debug.Log($"{gameObject.name} current health: {_currentHealth}/{_maxHealth}");

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateTextUI()
    {
        if (_healthTextDisplay != null)
        {
            _healthTextDisplay.text = $"{_currentHealth}/{_maxHealth}";
        }
    }

    private void Die()
    {
        Debug.Log($"💀 {gameObject.name} has been destroyed!");
        OnDeath?.Invoke();

        // Trigger the death animation parameter in our Animator!
        if (TryGetComponent<Animator>(out Animator animator))
        {
            animator.SetTrigger("Die");
        }

        // Disable the physics collider immediately so dead bodies can't block the player
        if (TryGetComponent<Collider>(out Collider col)) col.enabled = false;

        // Safely clear the object from the world AFTER the animation finishes playing (e.g., 2 seconds)
        Destroy(gameObject, 2.0f);
    }

}
