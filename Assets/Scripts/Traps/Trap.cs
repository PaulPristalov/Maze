using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;

    private TrapButton _defuseButton;
    private TrapButton _activateButton;

    private bool _active = true;

    private void Defuse()
    {
        _active = false;
    }

    private void Activate()
    {
        if (!_active)
            return;

        //TODO: shoot a projectile
    }
}
