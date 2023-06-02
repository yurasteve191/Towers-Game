using System.Collections;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    public DropperState dropperState;
    public Vector3 waveingPosition { get; private set; }
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] GameObject defaultDropperPoint;
    [SerializeField] GameObject hinglePoint;
    [SerializeField] GameObject blockPrefab;
    [SerializeField] GameObject currentBlock;
    [SerializeField] GameObject spawnNewBlockPoint;

    private void Awake()
    {
        SetWaveingPosition();
    }
    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine(CreateNewBlockCoroutine());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Drop();
        }
        SetLineRender();
    }
    private void SetWaveingPosition()
    {
        waveingPosition =  transform.position;
    }
    private void SetLineRender()
    {
        if (currentBlock == null)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        Vector3 direction = currentBlock.transform.position - lineRenderer.transform.position;
        direction.z = 0f;
        Quaternion rotation = Quaternion.LookRotation(direction);
        lineRenderer.transform.rotation = rotation;

        float distance = Vector3.Distance(currentBlock.transform.position, lineRenderer.transform.position);

        lineRenderer.positionCount = 2;

        Vector3[] linePoints = new Vector3[2];

        linePoints[0] = new Vector3(0, 0, 0);
        linePoints[1] = new Vector3(0, 0, distance);

        lineRenderer.SetPositions(linePoints);
    }

    private void CreateNewBlock()
    {
        GameObject newBlock = Instantiate(blockPrefab, spawnNewBlockPoint.transform.position, Quaternion.identity);
        newBlock.GetComponent<HingeJoint2D>().connectedBody = hinglePoint.GetComponent<Rigidbody2D>();
        currentBlock = newBlock;
    }

    public void Drop()
    {
        if(dropperState == DropperState.waveing)
        {
            transform.position = defaultDropperPoint.transform.position;
            currentBlock.GetComponent<HingeJoint2D>().enabled = false;
            currentBlock.GetComponent<BoxCollider2D>().isTrigger = false;
            currentBlock.GetComponent<Rigidbody2D>().freezeRotation = true;
            currentBlock = null;

            lineRenderer.positionCount = 0;

            dropperState = DropperState.switching;
            StartCoroutine(CreateNewBlockCoroutine());
        }
    }

    private IEnumerator CreateNewBlockCoroutine()
    {
        Vector3 underzonePoint = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);

        while (!Mathf.Approximately(Vector3.Distance(hinglePoint.transform.position, underzonePoint), 0f))
        {
            hinglePoint.transform.position = Vector3.MoveTowards(hinglePoint.transform.position, underzonePoint, 15 * Time.deltaTime);
            yield return null;
        }
        CreateNewBlock();
        yield return new WaitForSeconds(0.5f);
        while (!Mathf.Approximately(Vector3.Distance(hinglePoint.transform.position, transform.position), 0f))
        {
            hinglePoint.transform.position = Vector3.MoveTowards(hinglePoint.transform.position, transform.position, 15 * Time.deltaTime);
            yield return null;
        }
        dropperState = DropperState.waveing;
        StartCoroutine(WaweingCoroutine());
        yield return null;
    }
    private IEnumerator WaweingCoroutine()
    {

        StartCoroutine(Flying());
        while(dropperState == DropperState.waveing)
        {
            Rigidbody2D blockRb = currentBlock.GetComponent<Rigidbody2D>();
            if (currentBlock.GetComponent<HingeJoint2D>().jointSpeed < 10)
            {
                currentBlock.GetComponent<Rigidbody2D>().velocity = new Vector2(4, 0);
            }

            yield return new WaitForSeconds(2);
        }

        yield return null;
    }
    private IEnumerator Flying()
    {
        string direction = "up";
        float moveSpeed = 2f;
        float maxDifference = 2f;

        Vector3 upTargerPoint = new Vector3(transform.position.x, transform.position.y + maxDifference, transform.position.z);
        Vector3 downTargerPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);


        while (dropperState == DropperState.waveing)
        {
            if (direction == "up")
            {
                transform.position = Vector3.MoveTowards(transform.position, upTargerPoint, moveSpeed * Time.deltaTime);
                if (transform.position.y >= upTargerPoint.y)
                {
                    direction = "down";
                }
            }
            if (direction == "down")
            {
                transform.position = Vector3.MoveTowards(transform.position, downTargerPoint, moveSpeed * Time.deltaTime);
                if (transform.position.y <= downTargerPoint.y)
                {
                    direction = "up";
                }
            }
            yield return null;
        }
        yield return null;
    }
}
