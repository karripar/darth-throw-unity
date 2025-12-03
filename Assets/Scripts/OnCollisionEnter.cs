using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DartStick : MonoBehaviour
{
    private Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool hasSpawned = false; // Prevent multiple spawns

    [SerializeField] private float throwSpeed = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // Listen to release
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !hasSpawned)
        {
            hasSpawned = true;

            // 1. Stop interactable
            grabInteractable.enabled = false;

            // 2. Freeze dart
            rb.isKinematic = true;

            // 3. Align tip into wall
            Vector3 hitNormal = collision.contacts[0].normal;
            transform.rotation = Quaternion.LookRotation(-hitNormal);

            // 4. Parent to wall
            transform.SetParent(collision.transform);

            // 5. Spawn a new dart
            DartSpawner.Instance.SpawnNewDart();

            grabInteractable.selectExited.RemoveAllListeners();
        }
    }




    private void OnReleased(SelectExitEventArgs args)
    {
        if (!rb.isKinematic && !hasSpawned)
        {
            hasSpawned = true;
            DartSpawner.Instance.SpawnNewDart();
        }

        // Apply a simple tip-forward velocity based on the interactor's attach transform
        if (rb != null && args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor)
        {
            rb.linearVelocity = interactor.attachTransform.forward * throwSpeed;
            rb.angularVelocity = Vector3.zero;
        }

        grabInteractable.selectExited.RemoveAllListeners();
    }
}
