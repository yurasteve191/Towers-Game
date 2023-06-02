using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Character _character;
    public PlayerRotationState playerRotationState;
    public PlatformState platformState;
    private bool _isPlayerOnPlatform;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveDistance;

    private bool _isMovingRight = true;

    private Vector3 _startPosition;
    private GameManager gameManager;
    public bool isGivedScore { get; private set; }
    public int score = 5;

    public void SetGameManager()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void GiveScore()
    {
        if (!isGivedScore)
        {
            gameManager.ScoreAdd(this);
            isGivedScore = true;
        }
    }
    private void Start()
    {
        SetGameManager();
        _character = GetComponent<Character>();

        _startPosition = transform.position;

        switch (platformState)
        {
            case PlatformState.move:
                StartCoroutine(MovePlatform());
                break;
            case PlatformState.drop:
                _isPlayerOnPlatform = false;
                break;
            case PlatformState.simple:
                break;
        }
    }

    public PlayerRotationState GetRotationState()
    {
        return playerRotationState;
    }

    private IEnumerator MovePlatform()
    {
        //float targetX = _isMovingRight ? _startPosition.x + _moveDistance : _startPosition.x - _moveDistance;
        //Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        //while (transform.position != targetPosition)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * _moveSpeed);

            yield return null;
        //}

        //_isMovingRight = !_isMovingRight;

        //StartCoroutine(MovePlatform());
    }

    private IEnumerator DropPlatform()
    {
        yield return new WaitForSeconds(2f);

        //StopCoroutine(_character.StayOnMovingPlatform(_character.CoroutinePlatform));

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (platformState == PlatformState.drop && collision.gameObject.CompareTag("Player"))
        {
            _isPlayerOnPlatform = true;
            StartCoroutine(DropPlatform());
        }
    }


}