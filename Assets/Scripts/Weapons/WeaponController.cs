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
    [SerializeField] private int maxAmmo;
    [SerializeField] private int chargerSize;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    void Start()
    {
        ammo = chargerSize;
        UIManager.Instance.SetAmmo(ammo);
        UIManager.Instance.SetMaxAmmo(maxAmmo);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            Shoot();

        if (Keyboard.current.rKey.wasPressedThisFrame)
            Reload();

        if (Keyboard.current.fKey.wasPressedThisFrame)
            Inspect();
    }

    void Shoot()
    {
        if (ammo > 0)
        {
            ammo--;
            UIManager.Instance.SetAmmo(ammo);
            ShootRaycastFromCenter();
        }
    }

    void Reload()
    {
        if (maxAmmo > 0 && ammo < chargerSize)
            weaponAnimationController.Reload();
    }

    void Inspect()
    {
        weaponAnimationController.Inspect();
    }

    void ShootRaycastFromCenter()
    {
        weaponAnimationController.Shoot();

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;
        float maxDistance = 100f;

        Vector3 hitDirection = Vector3.zero;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            hitDirection = -ray.direction.normalized;

            if (hit.transform.CompareTag("Enemy"))
            {
                Quaternion reverseRotation = Quaternion.LookRotation(hitDirection);
                EnemyController enemy = hit.transform.GetComponent<EnemyController>();
                if (enemy != null)
                    enemy.TakeDamage(10, hit.point, reverseRotation);
            }

            if (hitParticleSystem != null)
            {
                hitParticleSystem.transform.position = hit.point;
                hitParticleSystem.transform.rotation = Quaternion.LookRotation(hitDirection);
                hitParticleSystem.Play();
            }
        }
    }

    public void RequestReload()
    {
        if (ammo >= chargerSize || maxAmmo <= 0)
            return;

        int spaceLeft = chargerSize - ammo;
        int bulletsToLoad = Mathf.Min(spaceLeft, maxAmmo);

        ammo += bulletsToLoad;
        maxAmmo -= bulletsToLoad;

        UIManager.Instance.SetAmmo(ammo);
        UIManager.Instance.SetMaxAmmo(maxAmmo);
    }
}