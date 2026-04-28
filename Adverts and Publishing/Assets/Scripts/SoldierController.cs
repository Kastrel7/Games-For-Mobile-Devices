using UnityEngine;

public class SoldierController : MonoBehaviour
{
    public GameObject bulletPrefab;
    
    void Start()
    {
        InvokeRepeating("FireBullet", 1f, 1f);
    }

    void FireBullet()
    {
        Instantiate(bulletPrefab, transform.position + new Vector3(0, 0, 1), Quaternion.Euler(new Vector3(90, 0, 0)));
    }
}
