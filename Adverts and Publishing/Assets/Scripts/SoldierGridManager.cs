using UnityEngine;
using System.Collections.Generic;

public class SoldierGridManager : MonoBehaviour
{
    public static SoldierGridManager Instance;

    public GameObject soldierPrefab;
    public float spacing = 1.5f;
    public float speed = 50f;

    private List<GameObject> soldiers = new List<GameObject>();
    private int mostSoldiers = 0;

    private Rigidbody rb;

    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();
        // Spawn the first soldier as a child instead of adding the manager itself
        GameObject firstSoldier = Instantiate(soldierPrefab, transform.position, Quaternion.identity, transform);
        soldiers.Add(firstSoldier);
        ArrangeGrid();
        UpdateCollider();
    }

    void OnEnable()
    {
        RewardedAdManager.OnRewardGranted += OnRewardGranted;
    }

    void OnDisable()
    {
        RewardedAdManager.OnRewardGranted -= OnRewardGranted;
    }

    void OnRewardGranted()
    {
        AddSoldiers(10);
        GameManager.Instance.ResumeGame();
    }

    public void AddSoldiers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Spawn as child of this GameObject using transform as parent
            GameObject newSoldier = Instantiate(soldierPrefab, transform.position, Quaternion.identity, transform);
            soldiers.Add(newSoldier);
        }

        if (soldiers.Count > mostSoldiers)
        {
            mostSoldiers = soldiers.Count;
        }

        ArrangeGrid();
        UpdateCollider();
    }

    public int GetMostSoldiers()
    {
        return mostSoldiers;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.transform.position.y));
            rb.linearVelocity = new Vector3((touchPos.x - transform.position.x) * speed, rb.linearVelocity.y, rb.linearVelocity.z);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, rb.linearVelocity.z);
        }
    }

    void ArrangeGrid()
    {
        int total = soldiers.Count;
        for (int i = 0; i < total; i++)
        {
            int col = i % 3;
            int row = i / 3;
            // Use localPosition since they're children
            soldiers[i].transform.localPosition = new Vector3(
                (col - 1) * spacing,
                0,
                -row * spacing
            );
        }
    }

    void UpdateCollider()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        int rows = Mathf.CeilToInt(soldiers.Count / 3f);
        int cols = Mathf.Min(soldiers.Count, 3);

        col.size = new Vector3(cols * spacing, 1f, rows * spacing);

        float xCenter = (cols - 1) / 2f * spacing - spacing;
        float zCenter = -(rows - 1) * spacing / 2f;
        col.center = new Vector3(xCenter, 0f, zCenter);
    }

    public void SolidiersLost()
    {
        foreach (GameObject soldier in soldiers)
        {
            Destroy(soldier);
        }
        soldiers.Clear();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Barrier")
            rb.linearVelocity = Vector3.zero;
    }
}