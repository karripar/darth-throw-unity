using UnityEngine;


public class DartSpawner : MonoBehaviour
{
    public static DartSpawner Instance;
    public GameObject dartPrefab;
    public Transform spawnPoint; // Where the dart sits on the table

    void Awake()
    {
        Instance = this;
    }

    public void SpawnNewDart()
    {
        // Spawn dart slightly above the table to avoid collider intersection
        Vector3 spawnPos = spawnPoint.position + new Vector3(0, 0.02f, 0);
        GameObject newDart = Instantiate(dartPrefab, spawnPos, spawnPoint.rotation);

        // Reset physics
        Rigidbody rb = newDart.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;        // Enable physics
            rb.linearVelocity = Vector3.zero;    // Reset movement
            rb.angularVelocity = Vector3.zero; // Reset rotation
        }

        // Ensure XR Grab Interactable is enabled
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = newDart.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != null) grab.enabled = true;
    }
}
