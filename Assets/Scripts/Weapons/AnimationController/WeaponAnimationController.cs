using System.Collections;
using UnityEngine;

enum WeaponState
{
    Idle,
    Shooting,
    Reloading
}

public class WeaponAnimationController : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioSource reloadSound;
    [SerializeField] private ParticleSystem bulletParticles;
    [SerializeField] private GameObject muzzleFlash;

    private WeaponController weaponController;
    private WeaponState state;
    private bool hasShot = false;

    private float smoothedSpeed = 0f;
    [SerializeField] private float speedSmoothing = 2f;


    void Start()
    {
        animator = GetComponent<Animator>();
        state = WeaponState.Idle;
        weaponController = GetComponentInParent<WeaponController>();
    }

    private void Update()
    {
        SetSpeed(PlayerController.Instance.GetVelocity());
    }

    public void Shoot()
    {
        hasShot = true;
        reloadSound.Stop();
        animator.Play("Shoot");
        shootSound.Play();
        bulletParticles.Emit(1);
        StartCoroutine(HandleMuzzleFlash());
    }

    public void Reload()
    {
        StartCoroutine(StartReloading());
        animator.Play("Reload");
        reloadSound.Play();
    }

    public void Inspect()
    {
        animator.Play("Inspect");
    }

    public void SetSpeed(float speed)
    {
        smoothedSpeed = Mathf.Lerp(smoothedSpeed, speed, Time.deltaTime * speedSmoothing);
        animator.SetFloat("speed", smoothedSpeed);
    }

    IEnumerator HandleMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        muzzleFlash.SetActive(false);
    }

    IEnumerator StartReloading()
    {
        hasShot = false;
        yield return new WaitForSeconds(1.2f);
        if (!hasShot)
            weaponController.RequestReload();
    }
}