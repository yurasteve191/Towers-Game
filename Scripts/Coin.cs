using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private GameManager gameManager;
    public int sum = 0;

    public void Start()
    {
        SetGameManager();
        StartCoroutine(Flying());
    }
    public void SetGameManager()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void Give()
    {
        gameManager.CoinAdd(this);
        Destroy(gameObject);
    }

    private IEnumerator Flying()
    {
        string direction = "up";
        float moveSpeed = 0.2f;
        float maxDifference = 0.2f;

        Vector3 upTargerPoint = new Vector3(transform.position.x, transform.position.y + maxDifference, transform.position.z);
        Vector3 downTargerPoint = new Vector3(transform.position.x, transform.position.y - maxDifference, transform.position.z);


        while (true)
        {
            if(direction == "up")
            {
                transform.position = Vector3.MoveTowards(transform.position, upTargerPoint, moveSpeed * Time.deltaTime);
                if (transform.position.y >= upTargerPoint.y)
                {
                    direction = "down";
                }
            }
            if(direction == "down")
            {
                transform.position = Vector3.MoveTowards(transform.position, downTargerPoint, moveSpeed * Time.deltaTime);
                if (transform.position.y <= downTargerPoint.y)
                {
                    direction = "up";
                }
            }
            yield return null;
        }
    }
}
