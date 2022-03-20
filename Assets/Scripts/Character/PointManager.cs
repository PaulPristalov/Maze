using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Grid _grid;

    private GameObject _startPoint;
    private GameObject _finishPoint;
    private Vector3 _finishCellPosition;

    [SerializeField] private GameObject _winPlaceholder;

    public void SetPoints(GameObject startPoint, GameObject finishPoint)
    {
        _startPoint = startPoint;
        _finishPoint = finishPoint;
        _finishCellPosition = _grid.PositionToCell(_finishPoint.transform.position);
    }

    private void Start()
    {
        SetPlayerOnStartPoint();
        _character.Health.Dying.AddListener(SetPlayerOnStartPoint);
    }

    private void Update()
    {
        if (_grid.PositionToCell(_character.transform.position) == _finishCellPosition)
            _winPlaceholder.SetActive(true);
    }

    private void SetPlayerOnStartPoint()
    {
        if (_startPoint == null)
            return;

        _character.transform.position = _startPoint.transform.position + new Vector3(0, _character.transform.position.y);
    }
}
