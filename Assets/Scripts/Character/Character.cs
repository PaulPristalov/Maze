using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Character : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _distanceToNext = 0.1f;
    private Vector3[] _path;

    [SerializeField] private Transform _model;
    [SerializeField] private float _rotationSpeed;

    public HealthManager Health { get; private set; }

    public UnityEvent<Vector3> OnClicked;

    private void Awake()
    {
        Health = GetComponent<HealthManager>();
        Health.Dying.AddListener(OnDying);
    }

    private void OnMouseDown()
    {
        if (_path != null)
            throw new System.Exception("You can't draw the path while character is moving.");

        OnClicked?.Invoke(transform.position);
    }

    public void SetPath(Vector3[] positions)
    {
        if (_path != null)
            throw new System.Exception("Path already exist.");

        _path = positions;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        int pathPoint = 0;

        while (_path != null && pathPoint < _path.Length)
        {
            Step(ref pathPoint);

            yield return null;
        }

        transform.position = _path[_path.Length - 1];
        _path = null;
    }

    private void Step(ref int pathPoint)
    {
        Vector3 direction = _path[pathPoint] - transform.position;
        Vector3 movementVector = new Vector3(direction.x, 0, direction.z);
        transform.Translate(movementVector.normalized * _movementSpeed * Time.deltaTime);

        if (movementVector.magnitude <= _distanceToNext)
        {
            pathPoint++;
            Rotate(pathPoint, movementVector);
        }
    }

    private void Rotate(int pathPoint, Vector3 currentMovementVector)
    {
        if (pathPoint >= _path.Length)
            return;

        Vector3 nextDirection = _path[pathPoint] - transform.position;
        Vector3 nextMovementVector = new Vector3(nextDirection.x, 0, nextDirection.z).normalized;
        if (currentMovementVector.normalized != nextMovementVector)
        {
            StartCoroutine(StartRotation(nextMovementVector));
        }
    }

    private IEnumerator StartRotation(Vector3 direction)
    {
        float startRotation = Mathf.Round(_model.localEulerAngles.y);
        float targetRotation = GetTargetRotation(direction);
        float rotationState = 0;

        while (TryRotate(startRotation, targetRotation, ref rotationState))
        {
            yield return null;
        }
    }

    private float GetTargetRotation(Vector3 direction)
    {
        Vector3 roundedDirection = new Vector3(Mathf.Round(direction.x), 0, Mathf.Round(direction.z) * -1);
        return Mathf.Atan2(roundedDirection.z, roundedDirection.x) * Mathf.Rad2Deg;
    }

    private bool TryRotate(float startRotationY, float targetRotationY, ref float state)
    {
        RotationStep(startRotationY, targetRotationY, state);
        state += Time.deltaTime * _rotationSpeed;

        if (state >= 1)
        {
            RotationStep(startRotationY, targetRotationY, 1);
            return false;
        }

        return true;
    }

    private void RotationStep(float startRotationY, float targetRotationY, float state)
    {
        _model.rotation = Quaternion.Lerp(Quaternion.Euler(0, startRotationY, 0), Quaternion.Euler(0, targetRotationY, 0), state);
    }

    private void OnDying()
    {
        _path = null;
    }
}
