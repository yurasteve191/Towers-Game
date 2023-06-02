using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockState blockState;
    [SerializeField] bool isBlockOn = false;

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 5 * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Block":
                if (!isBlockOn)
                {
                    Debug.Log(Vector3.Distance(transform.position, collision.gameObject.transform.position));
                    if(Vector3.Distance(transform.position, collision.gameObject.transform.position) < 0.9f)
                    {
                        StartCoroutine(SetBlockInCenterCoroutine(collision.gameObject));
                    }
                }
                break;
            case "Platform":
                if (!isBlockOn)
                {
                    isBlockOn = true;
                    Rigidbody2D blockRb = gameObject.GetComponent<Rigidbody2D>();
                    blockRb.velocity = new Vector2(0, 0);
                    blockRb.bodyType = RigidbodyType2D.Static;
                }
                break;

            default:
                break;
        }
    }

    private IEnumerator SetBlockInCenterCoroutine(GameObject collisionBlock)
    {
        isBlockOn = true;
        Rigidbody2D blockRb = gameObject.GetComponent<Rigidbody2D>();
        blockRb.velocity = new Vector2(0, 0);
        blockRb.bodyType = RigidbodyType2D.Static;


        Vector3 centerPoint = new Vector3(collisionBlock.transform.position.x, collisionBlock.transform.position.y + 0.8f, transform.position.z);

        while (transform.position.x != collisionBlock.transform.position.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, centerPoint, 20 * Time.deltaTime);
            yield return null;
        }

        yield return null;
    }

}
