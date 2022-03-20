using UnityEngine;
using UnityEngine.Events;

public abstract class TapInput : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private Camera _camera;

    public UnityEvent OnTapDown;
    public UnityEvent OnTapUp;

    public Grid Grid => _grid;

    private void Update()
    {
        CheckEvents();
        HandleTap();
    }

    private void CheckEvents()
    {
        if (Input.GetMouseButtonDown(0))
            OnTapDown?.Invoke();
        else if (Input.GetMouseButtonUp(0))
            OnTapUp?.Invoke();
    }

    private void HandleTap()
    {
        if (!Input.GetMouseButton(0))
            return;

        Vector3 tapPosition = GetTapPosition();
        ActionWithTap(tapPosition);
    }

    private Vector3 GetTapPosition()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, int.MaxValue))
        {
            Debug.DrawLine(Vector3.zero, hit.point, Color.red);
            return hit.point;
        }

        throw new System.Exception("Ray doesn't hit anything");
    }

    protected Vector3 GetCellPosition(Vector3 tapPosition) => Grid.PositionToCell(tapPosition);

    protected abstract void ActionWithTap(Vector3 tapPosition);
}
