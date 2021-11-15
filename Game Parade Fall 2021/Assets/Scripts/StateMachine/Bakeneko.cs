using UnityEngine;

public class Bakeneko:MonoBehaviour
{
    [SerializeField] float _chaseMoveSpeed = 10f;
    [SerializeField] float _neutralMoveSpeed = 6f;
    [SerializeField] float _catchDistance = 1f;
    [SerializeField] float _sightRange = 10f;
    [SerializeField] float _sightAngle = 45f;
    [SerializeField] LayerMask _obstaclesLayer;
    [SerializeField] Transform _eyes;

    public float NeutralSpeed => _neutralMoveSpeed;
    public float ChaseSpeed => _chaseMoveSpeed;
    public float CatchDistance => _catchDistance;

    Transform _playerTranform;
    float _sightRangeSqr;
    private void Awake()
    {
        _sightRangeSqr = _sightRange * _sightRange;
    }

    public bool CanSee()
    {
        _playerTranform = GameManager.Instance.PlayerTransform;
        if (_playerTranform == null)
            return false;
        Vector3 targetPosition = _playerTranform.transform.position;
        Vector3 targetDir = targetPosition.FlatVector() - transform.position.FlatVector();
        float angle = Vector3.Angle(targetDir.FlatVector(), _eyes.transform.forward.FlatVector());
        float distanceSqr = targetPosition.FlatVectorDistanceSquared(_eyes.transform.position);
        if (distanceSqr < _sightRangeSqr && angle < _sightAngle)
        {
            RaycastHit hit;
            float distance = Mathf.Sqrt(distanceSqr);
            Physics.Raycast(_eyes.position, targetDir.normalized, out hit, distance, _obstaclesLayer);
            if (hit.collider == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Color color = new Color(0, 0, 1, 0.25f);
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawSolidArc(_eyes.transform.position, Vector3.up, Quaternion.Euler(0, -_sightAngle, 0) * transform.forward, _sightAngle * 2f, _sightRange);
    }
#endif
}
