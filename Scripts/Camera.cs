using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public bool followX = true, followY = true;
    public float offcetX, offcetY;

    private Character character;
    [SerializeField] float followSpeed;

    private void Awake()
    {
        character = FindFirstObjectByType<Character>();
    }

    private void FixedUpdate()
    {
        Following();
    }

    private void Following()
    {
        Vector3 targetPosition;

        if (followX && followY)
        {
            targetPosition = new Vector3(character.transform.position.x + offcetX, character.transform.position.y + offcetY, -1);
        }else if (followY)
        {
            targetPosition = new Vector3(transform.position.x, character.transform.position.y + offcetY, -1);
        }else if (followX)
        {
            targetPosition = new Vector3(character.transform.position.x + offcetX, transform.position.y , -1);
        }
        else
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
