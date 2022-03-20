using UnityEngine;

public class TapInputCharacter : TapInput
{
    [SerializeField] private LineDrawer _drawer;

    protected override void ActionWithTap(Vector3 tapPosition)
    {
        Vector3 cellPosition = GetCellPosition(tapPosition);
        _drawer.DrawLine(cellPosition);
    }
}
