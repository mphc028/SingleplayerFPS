using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class WeaponController : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField] private WeaponAnimationController weaponAnimationController;

    [Header("Particle Settings")]
    [SerializeField] private ParticleSystem hitParticleSystem;

    private int ammo;
    private int maxAmmo;
    private int chargerSize;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ShootRaycastFromCenter();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Reload();
        }
    }

    private void Reload()
    {
        weaponAnimationController.Reload();
    }

    void ShootRaycastFromCenter()
    {
        weaponAnimationController.Shoot();

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f)); // Fixed: use 0.5f for z
        RaycastHit hit;

        float maxDistance = 100f;

        Vector3 endPoint;
        Vector3 hitDirection = Vector3.zero;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            endPoint = hit.point;

            // Calculate direction OPPOSITE to ray direction
            // This is from hit point back towards the camera
            hitDirection = -ray.direction.normalized;

            Debug.Log($"Hit: {hit.transform.name} | Ray Direction: {ray.direction} | Reverse Direction: {hitDirection}");

            if (hit.transform.CompareTag("Enemy"))
            {
                // Create rotation that faces the opposite direction of the ray
                Quaternion reverseRotation = Quaternion.LookRotation(hitDirection);

                // Pass the reverse rotation to the TakeDamage method
                EnemyController enemy = hit.transform.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(10, hit.point, reverseRotation);
                }
            }

            // Spawn hit particles at the impact point with reverse direction
            if (hitParticleSystem != null)
            {
                // Position at hit point
                hitParticleSystem.transform.position = hit.point;

                // Make particles go opposite to ray direction (back toward shooter)
                hitParticleSystem.transform.rotation = Quaternion.LookRotation(hitDirection);

                // Play the particle system
                hitParticleSystem.Play();

                Debug.DrawRay(hit.point, hitDirection * 5f, Color.green, 2f); // Visualize reverse direction
            }
        }
        else
        {
            endPoint = ray.origin + ray.direction * maxDistance;
        }

        // Visualize the ray for debugging
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red, 1f);

        /*
        // Draw in-game line
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, endPoint);
        */
    }
}