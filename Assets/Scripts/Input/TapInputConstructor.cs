using UnityEngine;

public class TapInputConstructor : TapInput
{
    [SerializeField] private WallPlacer _placer;

    protected override void ActionWithTap(Vector3 tapPosition)
    {
        _placer.Place(tapPosition);
    }
}
