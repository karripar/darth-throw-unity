using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DartStick : MonoBehaviour
{
    private Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    private bool hasSpawned = false; // prevent multiple spawns
    private bool released = false;   // track if the dart was thrown

    [SerializeField] private float throwSpeed = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // Listen to release event
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        released = true;
        rb.isKinematic = false;

        // Apply tip-forward velocity
        if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor)
        {
            rb.linearVelocity = interactor.attachTransform.forward * throwSpeed;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            rb.linearVelocity = transform.forward * throwSpeed;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only stick to wall and spawn once
        if (collision.gameObject.CompareTag("Wall") && !hasSpawned)
        {
            hasSpawned = true;

            // Stop physics
            rb.isKinematic = true;
            grabInteractable.enabled = false;

            // Stick tip-first
            Vector3 hitNormal = collision.contacts[0].normal;
            transform.rotation = Quaternion.LookRotation(-hitNormal) * Quaternion.Euler(0, 90, 0);

            // Parent to wall
            transform.SetParent(collision.transform);

            // Spawn next dart
            DartSpawner.Instance.SpawnNewDart();
        }
        else if (released && !hasSpawned)
        {
            // Dart hit something else after throw, still spawn next dart
            hasSpawned = true;
            DartSpawner.Instance.SpawnNewDart();
        }
    }
}
