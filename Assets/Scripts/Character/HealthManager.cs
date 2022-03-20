using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public UnityEvent Dying;

    public void Kill()
    {
        Dying?.Invoke();
    }
}
