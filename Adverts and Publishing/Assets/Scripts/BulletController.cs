using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.up * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z >= 50)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
