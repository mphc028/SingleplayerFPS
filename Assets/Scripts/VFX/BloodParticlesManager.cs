using UnityEngine;

public class BloodParticlesManager : MonoBehaviour
{
    public static BloodParticlesManager Instance;
    [SerializeField] private ParticleSystem particles;

    void Awake()
    {
        Instance = this;
    }

    public void PlayParticlesAt(Vector3 position, Quaternion rotation)
    {
        particles.transform.position = position;
        particles.transform.rotation = rotation;

        particles.Play();
    }
}
