﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public static float bulletForce = 10f;
    public int numberOfShoots = 1;
    GameObject currentHead;

    float moveSpeedInitial;

    void Start() {
        currentHead = GameObject.FindGameObjectWithTag("CurrentHead");
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2") && numberOfShoots > 0 && currentHead.activeSelf)
        {
            moveSpeedInitial =  PlayerBehaviour.moveSpeed;
            PlayerBehaviour.moveSpeed = 0;
        }
        if(Input.GetButtonUp("Fire2") && numberOfShoots > 0 && currentHead.activeSelf){
            Shoot();
            PlayerBehaviour.moveSpeed = moveSpeedInitial;
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
