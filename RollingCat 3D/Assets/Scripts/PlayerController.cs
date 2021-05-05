using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rollDuration = 1f;
    public LayerMask contactWallLayer;
    public DominantAxis dominantAxis { get; private set; }

    private Vector2 halfScale;
    public bool isRolling { get; private set; }
    public bool canMove;
    public Transform[] forcePoints;
    [SerializeField] private Transform ghostPlayer, pivot;

    public static PlayerController Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        canMove = false;
        dominantAxis = GetDominantAxis(transform);
        Vector3 colliderSize = GetComponent<BoxCollider>().size;
        halfScale = new Vector2(colliderSize.x, colliderSize.y) / 2;
    }

    public IEnumerator RollToDirection(Direction swipeDirection)
    {
        if (!isRolling && canMove)
        {
            isRolling = true;

            Vector2 movementDifference = Vector2.zero;
            float elapsedTime = 0f;
            float angle = 90f;
            Vector2 pivotOffset = Vector2.zero;

            Vector3 axis = Vector3.zero;
            Vector3 direction = Vector3.zero;
            Vector3 pivotPoint = transform.position;

            switch (dominantAxis)
            {
                case DominantAxis.X:
                    if (swipeDirection == Direction.Left || swipeDirection == Direction.Right)
                    {
                        movementDifference = new Vector2(0.5f, 0.5f);
                        pivotOffset = new Vector2(halfScale.y, halfScale.x);
                    }
                    else
                    {
                        movementDifference = Vector2.zero;
                        pivotOffset = Vector2.one * halfScale.x;
                    }
                    break;
                case DominantAxis.Y:
                    pivotOffset = halfScale;
                    movementDifference = new Vector2(0.5f, -0.5f);
                    break;
                case DominantAxis.Z:
                    if (swipeDirection == Direction.Up || swipeDirection == Direction.Down)
                    {
                        movementDifference = new Vector2(0.5f, 0.5f);
                        pivotOffset = new Vector2(halfScale.y, halfScale.x);
                    }
                    else
                    {
                        movementDifference = Vector2.zero;
                        pivotOffset = Vector2.one * halfScale.x;
                    }
                    break;
                default:
                    break;
            }

            switch (swipeDirection)
            {
                case Direction.Left:
                    direction = Vector3.left;
                    axis = Vector3.forward;
                    pivotPoint += (Vector3.left * pivotOffset.x);
                    break;
                case Direction.Up:
                    direction = Vector3.forward;
                    axis = Vector3.right;
                    pivotPoint += (Vector3.forward * pivotOffset.x);
                    break;
                case Direction.Right:
                    direction = Vector3.right;
                    axis = Vector3.back;
                    pivotPoint += (Vector3.right * pivotOffset.x);
                    break;
                case Direction.Down:
                    direction = Vector3.back;
                    axis = Vector3.left;
                    pivotPoint += (Vector3.back * pivotOffset.x);
                    break;
                default:
                    break;
            }

            pivotPoint -= (Vector3.up * pivotOffset.y);
            pivot.position = pivotPoint;

            //simulate before the action in order to get an ideal result
            CopyTransformData(ghostPlayer);
            ghostPlayer.RotateAround(pivotPoint, axis, angle);
            Quaternion endRotation = ghostPlayer.rotation;
            Vector3 endPosition = transform.position + direction * (1 + movementDifference.x) + (Vector3.up * movementDifference.y);

            while (elapsedTime < rollDuration)
            {
                elapsedTime += Time.deltaTime;

                transform.RotateAround(pivotPoint, axis, (angle / rollDuration) * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }

            dominantAxis = GetDominantAxis(transform);
            transform.rotation = endRotation;
            transform.position = endPosition;

            Debug.Log("Rolled to the " + swipeDirection);
            Debug.Log("Dominant Axis: " + dominantAxis);
            isRolling = false;
            
            pivotPoint = transform.position;
            pivot.position = pivotPoint;
        }
    }

    void CopyTransformData(Transform target)
    {
        target.localPosition = transform.localPosition;
        target.localEulerAngles = transform.localEulerAngles;
        target.localScale = transform.localScale;
    }

    DominantAxis GetDominantAxis(Transform transform)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 50f, contactWallLayer))
        {
            switch (hit.collider.name)
            {
                case "X":
                    return DominantAxis.X;
                case "Y":
                    return DominantAxis.Y;
                case "Z":
                    return DominantAxis.Z;
                default:
                    return DominantAxis.None;
            }
        }
        return DominantAxis.None;
    }

    public void AddForce(int pointIndex)
    {
        GetComponent<Rigidbody>().constraints =  RigidbodyConstraints.None;
        GetComponent<Rigidbody>().AddForceAtPosition(-Vector3.up * 100f, forcePoints[pointIndex].position, ForceMode.Force);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3[] directions = { transform.up, -transform.up, transform.forward, -transform.forward, transform.right, -transform.right };

        for (int i = 0; i < directions.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, directions[i]);
        }
    }
}

public enum DominantAxis { X, Y, Z, None }