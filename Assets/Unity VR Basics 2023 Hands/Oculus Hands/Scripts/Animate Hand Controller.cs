using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class AnimateHandController : MonoBehaviour
{
    [Header("Input Actions")]
    [Tooltip("Input action reference for the grip control.")]
    [SerializeField] private InputActionReference gripInputActionReference;

    [Tooltip("Input action reference for the trigger control.")]
    [SerializeField] private InputActionReference triggerInputActionReference;

    private Animator _handAnimator; // Animator component for the hand model.
    private float _gripValue; // Current value of the grip input.
    private float _triggerValue; // Current value of the trigger input.

    /// <summary>
    /// Initialize the animator component at the start.
    /// </summary>
    private void Start()
    {
        _handAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// Update is called once per frame to animate hand controls.
    /// </summary>
    private void Update()
    {
        UpdateGripAnimation();
        UpdateTriggerAnimation();
    }

    /// <summary>
    /// Updates the grip animation parameter based on the grip input value.
    /// </summary>
    private void UpdateGripAnimation()
    {
        _gripValue = gripInputActionReference.action?.ReadValue<float>() ?? 0f;
        _handAnimator.SetFloat("Grip", _gripValue);
    }

    /// <summary>
    /// Updates the trigger animation parameter based on the trigger input value.
    /// </summary>
    private void UpdateTriggerAnimation()
    {
        _triggerValue = triggerInputActionReference.action?.ReadValue<float>() ?? 0f;
        _handAnimator.SetFloat("Trigger", _triggerValue);
    }
}
