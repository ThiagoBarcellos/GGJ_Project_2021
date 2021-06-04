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
    }

    float moveSpeedInitial;

    void Update()
    {
        if(GameObject.FindGameObjectWithTag("CurrentHead"))
            currentHead = GameObject.FindGameObjectWithTag("CurrentHead");

        if (Input.GetButtonDown("Fire2") && numberOfShoots > 0 && currentHead)
        {
            moveSpeedInitial = PlayerBehaviour.moveSpeed;
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
        Vector3 moveDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameObject.FindGameObjectWithTag("Player").transform.position);
        moveDirection.z = 0;       
        moveDirection.Normalize();
        Vector2 playerTransform = new Vector2(transform.position.x + CalculateOffset().x, transform.position.y + CalculateOffset().y);
        GameObject bulletInstance = Instantiate(bulletPrefab, playerTransform, Quaternion.Euler(new Vector3(0,0,0)));
        Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
        rb.AddForce(moveDirection * bulletForce, ForceMode2D.Impulse);

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

    Vector2 CalculateOffset(){
        Vector3 moveDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameObject.FindGameObjectWithTag("Player").transform.position);
        moveDirection.z = 0;       
        moveDirection.Normalize();
        float offsetDistance = 0.5f;
        Vector2 offset = new Vector2(0,0);
        if(moveDirection.x >= this.transform.position.x && moveDirection.y >= this.transform.position.y){
            //Primeiro quadrante
            offset = new Vector2(0,offsetDistance);
        }
        else if(moveDirection.x < this.transform.position.x && moveDirection.y >= this.transform.position.y){
            //Segundo quadrante
            offset = new Vector2(0,offsetDistance);
        }
        else if(moveDirection.x < this.transform.position.x && moveDirection.y < this.transform.position.y){
            //Terceiro quadrante
            offset = new Vector2(offsetDistance * -1,offsetDistance * -1);
        }
        else if(moveDirection.x >= this.transform.position.x && moveDirection.y < this.transform.position.y){
            //Quarto quadrante
            offset = new Vector2(offsetDistance,offsetDistance * -1);
        }

        return offset;
    }
}
