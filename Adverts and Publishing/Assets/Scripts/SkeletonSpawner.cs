using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    public GameObject skeletonPrefab;

    public int rows = 5;
    private int currentRow = 0;

    private float xDistance = 1.5f;
    private float zDistance = 5f;

    private GameObject skeleton1;

    void Start()
    {
        SpawnSkeleton();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentRow+1 < rows)
        {
            WaitForNextRow();
        }
    }

    private void SpawnSkeleton()
    {
        skeleton1 = Instantiate(skeletonPrefab, transform.position + new Vector3(xDistance, 0, 0), Quaternion.Euler(new Vector3(0, 180, 0)));
        GameManager.Instance.RegisterSkeleton();
        Instantiate(skeletonPrefab, transform.position - new Vector3(xDistance, 0, 0), Quaternion.Euler(new Vector3(0, 180, 0)));
        GameManager.Instance.RegisterSkeleton();
    }

    private void WaitForNextRow()
    {
        if (skeleton1 == null) return;
        float currentZDistance = transform.position.z - skeleton1.transform.position.z;
        if (currentZDistance >= zDistance)
        {
            currentRow++;
            SpawnSkeleton();
        }
    }
}
