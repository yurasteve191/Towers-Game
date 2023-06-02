using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public int resolution = 11;
    public LineRenderer lineRenderer { get; private set; }

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = resolution;
    }
    public void DrawTrajectory(Vector2 startPosition, Vector2 jumpVector)
    {
        lineRenderer.enabled = true;

        Vector3[] position = new Vector3[resolution];
        for (int i = 0; i<position.Length; i++)
        {
            float time = i * 0.1f;
            Vector3 pos = startPosition + jumpVector * time + 0.5f * Physics2D.gravity * time * time;

            position[i] = pos;
        }
        lineRenderer.SetPositions(position);
    }
    public void HideTrajectory()
    {
        lineRenderer.enabled = false;
    }

}
