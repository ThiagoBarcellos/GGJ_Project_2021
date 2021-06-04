using UnityEngine;
using System.Collections;

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
    public bool isShooting = false;

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
            IEnumerator Coroutine = shootAnim(0.1f);
            StartCoroutine(Coroutine);
            PlayerBehaviour.moveSpeed = moveSpeedInitial;
        }
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void Shoot()
    {
        currentHead.SetActive(false);
        Vector3 moveDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position);
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
        float offsetDistance = 0.5f;
        Vector2 offset = new Vector2(0,0);
        if(mousePos.x >= this.transform.position.x && mousePos.y >= this.transform.position.y){
            //Primeiro quadrante
            offset = new Vector2(0,offsetDistance);
        }
        else if(mousePos.x < this.transform.position.x && mousePos.y >= this.transform.position.y){
            //Segundo quadrante
            offset = new Vector2(0,offsetDistance);
        }
        else if(mousePos.x < this.transform.position.x && mousePos.y < this.transform.position.y){
            //Terceiro quadrante
            offset = new Vector2(offsetDistance * -1,offsetDistance * -1);
        }
        else if(mousePos.x >= this.transform.position.x && mousePos.y < this.transform.position.y){
            //Quarto quadrante
            offset = new Vector2(offsetDistance,offsetDistance * -1);
        }

        return offset;
    }

    private IEnumerator shootAnim(float waitTime){
        StartCoroutine(stopToShoot(0.4f));
        isShooting = true;
        yield return new WaitForSeconds(waitTime);
        Shoot();
        isShooting = false;
    }

    private IEnumerator stopToShoot(float waitTime){
        Player playerScript = this.GetComponent<Player>();
        float speed = playerScript.moveSpeed;
        playerScript.moveSpeed = 0;
        yield return new WaitForSeconds(waitTime);
        playerScript.moveSpeed = speed;
    }
}
