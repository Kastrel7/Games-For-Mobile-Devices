using UnityEngine;

public class SkeletonController : MonoBehaviour, IDamageable
{
    public float speed = 1.5f;
    public int maxHits = 5;

    private int currentHits = 0;

    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        if (transform.position.z <= -5)
        {
            GameManager.Instance.SkeletonsWin();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHits += amount;
        if (currentHits >= maxHits)
        {
            GameManager.Instance.SkeletonDied();
            Destroy(gameObject);
        }
    }
}
