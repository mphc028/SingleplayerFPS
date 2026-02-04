using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int health = 100;

    public void TakeDamage(int damage, Vector3 hitPosition, Quaternion hitRotation)
    {
        BloodParticlesManager.Instance.PlayParticlesAt(hitPosition, hitRotation);

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
