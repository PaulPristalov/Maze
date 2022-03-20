using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DeathZone : MonoBehaviour
{
    [SerializeField] private bool _destroyable;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out HealthManager health))
        {
            health.Kill();
        }

        if (_destroyable)
        {
            Destroy(gameObject);
        }
    }
}
