using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class ShootRaycastExample : MonoBehaviour
{
    private LineRenderer lineRenderer;
    
    [SerializeField] private WeaponAnimationController weaponAnimationController;

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

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        float maxDistance = 100f;

        Vector3 endPoint;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            endPoint = hit.point;
            Debug.Log("Hit: " + hit.transform.name);
        }
        else
        {
            endPoint = ray.origin + ray.direction * maxDistance;
        }

        /*
        // Draw in-game line
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, endPoint);
        */
    }
}
