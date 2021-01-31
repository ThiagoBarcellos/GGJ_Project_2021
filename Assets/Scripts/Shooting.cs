using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public static float bulletForce = 10f;
    public int numberOfShoots = 1;
    GameObject currentHead;

    void Start() {
        currentHead = GameObject.FindGameObjectWithTag("CurrentHead");
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && numberOfShoots > 0 && currentHead.activeSelf)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        currentHead.SetActive(false);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

        numberOfShoots--;
    }

    void CollectShoot(GameObject obj) {
        numberOfShoots = 1;
        currentHead.SetActive(true);
        Destroy(obj);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.CompareTag("Shoot")) {
            CollectShoot(col.gameObject);
        }
    }
}
