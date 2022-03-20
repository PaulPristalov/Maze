using UnityEngine;
using UnityEngine.Events;

public class TrapButton : MonoBehaviour
{
    public UnityEvent _onPressed;

    private void Press()
    {
        _onPressed?.Invoke();
    }
}
