using UnityEngine;


public class DartStick : MonoBehaviour
{
    private Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only stick to walls (layer mask optional)
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Make dart kinematic to "stick"
            rb.isKinematic = true;

            // Optional: parent it to the wall so it moves with it
            transform.SetParent(collision.transform);

            // Disable grabbing so it can't be grabbed again
            grabInteractable.enabled = false;

            // Spawn a new dart after a short delay
            DartSpawner.Instance.SpawnNewDart();
        }
    }
}
