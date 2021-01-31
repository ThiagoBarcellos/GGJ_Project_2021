using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public static float bulletForce = 10f;
    public int numberOfShoots = 1;
    GameObject currentHead;
    Rigidbody2D rb;
    Camera cam;
    Vector2 mousePos;


    void Start(){
        cam = Camera.main;
        rb = GameObject.FindGameObjectWithTag("FirePoint").GetComponent<Rigidbody2D>();
    }

    float moveSpeedInitial;

    void Update()
    {

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDirection = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        if(GameObject.FindGameObjectWithTag("CurrentHead"))
            currentHead = GameObject.FindGameObjectWithTag("CurrentHead");

        if (Input.GetButtonDown("Fire2") && numberOfShoots > 0 && currentHead)
        {
            moveSpeedInitial =  PlayerBehaviour.moveSpeed;
            PlayerBehaviour.moveSpeed = 0;
        }

        if (Input.GetButtonUp("Fire2") && numberOfShoots > 0 && currentHead){
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
        currentHead.SetActive(true);
        numberOfShoots = 1;
        Destroy(obj);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.CompareTag("Shoot")) {
            CollectShoot(col.gameObject);
        }
    }
}
