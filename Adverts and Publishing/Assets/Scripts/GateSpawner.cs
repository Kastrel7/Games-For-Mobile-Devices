using UnityEngine;

public class GateSpawner : MonoBehaviour
{
    public GameObject gatePrefab;

    private GameObject gate;

    void Start()
    {
        SpawnGate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnGate()
    {
        gate = Instantiate(gatePrefab, transform.position, Quaternion.identity);
    }

    private void WaitForCurrentGateToDie()
    {
        
    }
}
