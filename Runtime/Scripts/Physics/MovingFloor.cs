using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    [Header("Moving Settings")]
    [SerializeField] private float movingSpeed = 0.3f;
    [SerializeField] private Vector3[] movePositions;

    private Rigidbody rb;

    private int nextPosIndex = 0;
    private Vector3 startPos;
    private Vector3 endPos;
    private float moveTimer = 0f;
    private float moveDuration = 0f;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        nextPosIndex = 1;
        SetMove(movePositions[0], movePositions[nextPosIndex]);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        moveTimer += Time.fixedDeltaTime;

        if (moveTimer < moveDuration)
        {
            rb.MovePosition(Vector3.Lerp(startPos, endPos, moveTimer / moveDuration));
        }
        else
        {
            rb.MovePosition(endPos);
            nextPosIndex = (nextPosIndex + 1) % movePositions.Length;
            SetMove(endPos, movePositions[nextPosIndex]);
        }
    }

    private void SetMove(Vector3 startPos, Vector3 endPos)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        moveDuration = Vector3.Distance(startPos, endPos) / movingSpeed;
        moveTimer = 0f;
    }
}
