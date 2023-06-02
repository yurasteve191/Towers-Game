using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] SpriteRenderer skin;
    [SerializeField] float increaseSpeed = 10f;
    public Vector2 maxJumpVectorForce { get; private set; }
    public Vector2 jumpVector { get; private set; }

    public bool isPlayerOnPlatform { get; private set; }
    public bool isJumpVectorUp { get; private set; }
    public bool isOnMovingPlatform { get; private set; }

    public Trajectory trajectory { get; private set; }
    public Rigidbody2D rb { get; private set; }
    [SerializeField] PlayerRotationState playerRotationState;


    private Coroutine stayOnMovingPlatformCoroutine;

    private void Awake()
    {
        trajectory = GetComponentInChildren<Trajectory>();
        rb = GetComponent<Rigidbody2D>();

        SetPlayerRotationState(PlayerRotationState.right);
        SetDefaultJumpVector();
    }
    private void Start()
    {
        StartCoroutine(Flying());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetPlayerRotationState(PlayerRotationState.left);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SetPlayerRotationState(PlayerRotationState.right);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerRotationState == PlayerRotationState.left)
            {
                jumpVector = new Vector2(-1.5f, 6);
            }
            if (playerRotationState == PlayerRotationState.right)
            {
                jumpVector = new Vector2(1.5f, 6);
            }
            Jump();
        }
    }

    private void SetPlayerRotationState(PlayerRotationState state)
    {
        playerRotationState = state;
        SetDefaultJumpVector();
        SetSkinRotationState();
    }
    private void SetSkinRotationState()
    {
        if(playerRotationState == PlayerRotationState.left)
        {
            skin.flipX = true;
        }
        if(playerRotationState == PlayerRotationState.right)
        {
            skin.flipX = false;
        }
    }

    public void SetDefaultJumpVector()
    {
        maxJumpVectorForce = new Vector2(8, 7);
        jumpVector = new Vector2(0, maxJumpVectorForce.y);
    }
    public void GeneratingJumpVector()
    {
        if (!isPlayerOnPlatform)
        {
            return;
        }
        if (playerRotationState == PlayerRotationState.right)
        {
            if (!isJumpVectorUp)
            {
                jumpVector = new Vector2((jumpVector.x + Time.deltaTime * increaseSpeed), maxJumpVectorForce.y);
                if (jumpVector.x >= maxJumpVectorForce.x)
                {
                    isJumpVectorUp = true;
                }
            }
            if (isJumpVectorUp)
            {
                jumpVector = new Vector2((jumpVector.x - Time.deltaTime * increaseSpeed), maxJumpVectorForce.y);
                if (jumpVector.x <= 0)
                {
                    isJumpVectorUp = false;
                }
            }
        }

        if (playerRotationState == PlayerRotationState.left)
        {
            if (!isJumpVectorUp)
            {
                jumpVector = new Vector2((jumpVector.x - Time.deltaTime * increaseSpeed), maxJumpVectorForce.y);
                if (jumpVector.x <= (maxJumpVectorForce.x * -1))
                {
                    isJumpVectorUp = true;
                }
            }
            if (isJumpVectorUp)
            {
                jumpVector = new Vector2((jumpVector.x + Time.deltaTime * increaseSpeed), maxJumpVectorForce.y);
                if (jumpVector.x >= 0)
                {
                    isJumpVectorUp = false;
                }
            }
        }

        trajectory.DrawTrajectory((Vector2)transform.position, jumpVector);
    }
    public void Jump()
    {
        //if (isPlayerOnPlatform)
        //{
            //StopCoroutine(stayOnMovingPlatformCoroutine);
            rb.velocity = jumpVector;
        //}
    }


    private IEnumerator StayOnMovingPlatform(GameObject platform)
    {
        Vector3 distance = transform.position - platform.transform.position;
        while (true)
        {
            transform.position = platform.transform.position + distance;
            yield return null;
        }
    }
    private IEnumerator Flying()
    {
        string direction = "up";
        float moveSpeed = 0.1f;
        float maxDifference = 0.05f;


        while (true)
        {
            if (direction == "up")
            {
                Vector3 upTargerPoint = new Vector3(transform.position.x, transform.position.y + maxDifference, transform.position.z);
                skin.transform.position = Vector3.MoveTowards(skin.transform.position, upTargerPoint, moveSpeed * Time.deltaTime);
                if (skin.transform.position.y >= upTargerPoint.y)
                {
                    direction = "down";
                }
            }
            if (direction == "down")
            {
                Vector3 downTargerPoint = new Vector3(transform.position.x, transform.position.y - maxDifference, transform.position.z);
                skin.transform.position = Vector3.MoveTowards(skin.transform.position, downTargerPoint, moveSpeed * Time.deltaTime);
                if (skin.transform.position.y <= downTargerPoint.y)
                {
                    direction = "up";
                }
            }
            yield return null;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Platform":
                Platform platform = collision.gameObject.GetComponent<Platform>();
                SetPlayerRotationState(platform.GetRotationState());
                //stayOnMovingPlatformCoroutine = StartCoroutine(StayOnMovingPlatform(collision.gameObject));
                platform.GiveScore();
                isPlayerOnPlatform = true;
                break;
            default:
                break;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isPlayerOnPlatform = false;
        trajectory.HideTrajectory();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Coin":
                Coin coin = collider.gameObject.GetComponent<Coin>();
                coin.Give();
                break;

            default:
                break;
        }
    }


}