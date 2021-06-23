using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    #region Variables
    public Transform firePoint;
    public GameObject bulletPrefab;
    public static float bulletForce = 10f;
    public int numberOfShoots = 1;
    GameObject currentHead;
    Rigidbody2D rb;
    Camera cam;
    Vector2 mousePos;
    public bool isShooting = false, isAttacking = false;
    float moveSpeedInitial;
    public bool attackHorizontal = false, attackUp = false, attackDown = false;
    public Animator playerAnimator;
    private AnimatorClipInfo[] clipInfo;
    #endregion

    void Start(){
        playerAnimator = this.GetComponent<Animator>();
        cam = Camera.main;
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        ShootTrigger();
        AttackTrigger();
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


    #region Shoot Script
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
        float speed = Player.moveSpeed;
        Player.moveSpeed = 0;
        yield return new WaitForSeconds(waitTime);
        Player.moveSpeed = speed;
    }
    private void ShootTrigger(){
        if(GameObject.FindGameObjectWithTag("CurrentHead"))
            currentHead = GameObject.FindGameObjectWithTag("CurrentHead");

        if (Input.GetButtonDown("Fire2") && numberOfShoots > 0 && currentHead)
        {
            moveSpeedInitial = Player.moveSpeed;
            Player.moveSpeed = 0;
        }

        if (Input.GetButtonUp("Fire2") && numberOfShoots > 0 && currentHead){
            IEnumerator Coroutine = shootAnim(0.1f);
            StartCoroutine(Coroutine);
            Player.moveSpeed = moveSpeedInitial;
        }
    }
    #endregion

    #region Attack Script
    void Attack(){
        mousePos.x -= this.transform.position.x;
        mousePos.y -= this.transform.position.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        if(angle > -45 && angle <= 45) attackHorizontal = true;
        else if(angle > 45 && angle <= 135) attackUp = true;
        else if(angle > 135 && angle <= 180 || angle < -135 && angle >= -180) attackHorizontal = true;
        else if(angle > -135 && angle <= -45) attackDown = true;
    }
    private void AttackTrigger(){
        if(Input.GetMouseButtonDown(0)){
            StartCoroutine(AttackAnim(0.1f));
        }
    }
    private IEnumerator AttackAnim(float waitTime){
        //StartCoroutine(stopToAttack(0.2f));
        if(GetCurrentClipName("Player_FrontAttack")){
            float speed = Player.moveSpeed;
            Player.moveSpeed = 0;
            Player.moveSpeed = speed;
            isAttacking = true;
            Attack();
            yield return new WaitForSeconds(waitTime);
            isAttacking = false;
            attackHorizontal = false;
        }
        else if(GetCurrentClipName("Player_UpAttack")){
            float speed = Player.moveSpeed;
            Player.moveSpeed = 0;
            Player.moveSpeed = speed;
            isAttacking = true;
            Attack();
            yield return new WaitForSeconds(waitTime);
            isAttacking = false;
            attackUp = false;
        }
        else if(this.playerAnimator.GetCurrentAnimatorClipInfo(0).IsName("Player_DownAttack")){
            float speed = Player.moveSpeed;
            Player.moveSpeed = 0;
            Player.moveSpeed = speed;
            isAttacking = true;
            Attack();
            yield return new WaitForSeconds(waitTime);
            isAttacking = false;
            attackDown = false;
        }
    }
    private IEnumerator stopToAttack(float waitTime){
        float speed = Player.moveSpeed;
        Player.moveSpeed = 0;
        yield return new WaitForSeconds(waitTime);
        Player.moveSpeed = speed;
    }

    /*public bool GetCurrentClipName(string clipname){
        clipInfo = playerAnimator.GetCurrentAnimatorClipInfo(0);
        if(clipname.equals(clipInfo[0].clip.name))
            return true;
        else
            return false;
    }   */
    #endregion
}