using System.Collections;
using System.Linq;
using UnityEngine;

enum WeaponState
{
   Idle,
   Shooting,
   Reloading
}

public class WeaponAnimationController : MonoBehaviour
{

    private float speed = 0f;
    private Animator animator;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioSource reloadSound;
    [SerializeField] private ParticleSystem bulletParticles;
    [SerializeField] private GameObject muzzleFlash;

    private WeaponState state;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        state = WeaponState.Idle;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reload()
    {
        animator.Play("Reload");
        reloadSound.Play();
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void Shoot()
    {
        animator.Play("Shoot");
        shootSound.Play();
        bulletParticles.Emit(1);

        StartCoroutine(HandleMuzzleFlash());
    }


    IEnumerator HandleMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        muzzleFlash.SetActive(false);
    }

}
