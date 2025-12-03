using UnityEngine;


public class DartSpawner : MonoBehaviour
{
    public static DartSpawner Instance;
    public GameObject dartPrefab;
    public Transform spawnPoint;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Spawn the first dart on the table
        SpawnNewDart();
    }

    public void SpawnNewDart()
    {
        Vector3 spawnPos = spawnPoint.position + new Vector3(0, 0.02f, 0);
        GameObject newDart = Instantiate(dartPrefab, spawnPos, spawnPoint.rotation);

        // Reset Rigidbody
        Rigidbody rb = newDart.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        // Enable grab interactable
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = newDart.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != null)
            grab.enabled = true;
    }
}
