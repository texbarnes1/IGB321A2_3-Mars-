using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAvatar : MonoBehaviour {

    public GameObject avatar;
    private Animator anim;

    public float health = 100.0f;
    private bool dead = false;

    //Movement
    public float moveSpeed = 5;
    private Vector3 playerPosition;
    private Rigidbody rb;

    //Weapons
    public GameObject bullet;
    private float MGFireTime = 0.075f;
    private float MGFireTimer;
    public int ammo = 500;

    public GameObject fireDamage;
    private float FTFireTime = 0.2f;
    private float FTFireTimer;
    public int fuel = 50;

    //AddedMechanics
    public int randomJamMin;
    public int randomJamMax;
    public int bulletsUntilJam;
    public bool isJammed = false;
    public float jamTime;
    public int keyCards = 0;

    private AudioManager audioManager;

    // A UI popup that indicates the weapon has jammed
    public GameObject jamIndicator;

    //Weapon Effects
    public GameObject muzzleFlash;
    public GameObject flameStream;



    // Use this for initialization
    void Start () {
        anim = avatar.GetComponent<Animator>();
        flameStream.GetComponent<ParticleSystem>().Stop();
        rb = GetComponent<Rigidbody>();
        audioManager = FindObjectOfType<AudioManager>();
        bulletsUntilJam = RandomInt();
    }
	
	// Update is called once per frame
	void Update () {

        if (!GameManager.instance.playerDead) {
            Movement();
            Shooting();
        } 
    }

    void Shooting() {
        //Left Mouse Button
        if (Input.GetMouseButton(0) && ammo >= 1 && isJammed == false) {

            muzzleFlash.SetActive(true);

            if (Time.time > MGFireTimer) {
                audioManager.Play("GunShot");
                Instantiate(bullet, muzzleFlash.transform.position, transform.rotation);
                bulletsUntilJam -= 1;
                //ammo -= 1;
                MGFireTimer = Time.time + MGFireTime;
            }

            anim.SetBool("Shooting", true);
        }
        else {
            muzzleFlash.SetActive(false);
            anim.SetBool("Shooting", false);
        }

        if (isJammed == true)
        {
            WeaponJam();
        }
            

        if (bulletsUntilJam == 0 && isJammed == false)
        {
            //Debug.Log("this Triggered");
            isJammed = true;
        }



        //Right Mouse Button
        if (Input.GetMouseButtonDown(1) && fuel >= 1) {
            flameStream.GetComponent<ParticleSystem>().Play();
            anim.SetBool("Flamer", true);
        }
        else if (Input.GetMouseButtonUp(1) && fuel >= 1) {
            flameStream.GetComponent<ParticleSystem>().Stop();
            anim.SetBool("Flamer", false);
        }
        else if (fuel <= 0) {
            flameStream.GetComponent<ParticleSystem>().Stop();
            anim.SetBool("Flamer", false);
        }

        if (Input.GetMouseButton(1) && fuel >= 1) {
            if (Time.time > FTFireTimer) {
                audioManager.Play("FlameThrower");
                Instantiate(fireDamage, flameStream.transform.position, transform.rotation);
                fuel -= 1;
                FTFireTimer = Time.time + FTFireTime;
            }
        }
    }

    void Movement() {

        playerPosition = transform.position;

        //Forwards and Back
        if (Input.GetKey("w")) {
            playerPosition.z = playerPosition.z + moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("s")) {
            playerPosition.z = playerPosition.z - moveSpeed * Time.deltaTime;
        }

        //Strafing 
        if (Input.GetKey("a")) {
            playerPosition.x = playerPosition.x - moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("d")) {
            playerPosition.x = playerPosition.x + moveSpeed * Time.deltaTime;
        }

        //Animation Controls - Move Vector Dot Product
        Vector3 moveVector = (playerPosition - transform.position).normalized;
        float direction = Vector3.Dot(moveVector, transform.forward);

        //Relative Forwards
        if (direction > 0.8f) {
            anim.SetBool("Walking F", true);
            anim.SetBool("Walking B", false);
            anim.SetBool("Strafe R", false);
            anim.SetBool("Strafe L", false);
        }
        //Relative Right
        else if (direction < 0.8f && direction > 0) {
            anim.SetBool("Walking F", false);
            anim.SetBool("Walking B", false);
            anim.SetBool("Strafe R", true);
            anim.SetBool("Strafe L", false);
        }
        //Relative Backwards
        else if (direction < -0.8f) {
            anim.SetBool("Walking F", false);
            anim.SetBool("Walking B", true);
            anim.SetBool("Strafe R", false);
            anim.SetBool("Strafe L", false);
        }
        //Relative Left
        else if (direction > -0.8f && direction < 0) {
            anim.SetBool("Walking F", false);
            anim.SetBool("Walking B", false);
            anim.SetBool("Strafe R", false);
            anim.SetBool("Strafe L", true);
        }
        //Turn off all anims
        else {
            anim.SetBool("Walking F", false);
            anim.SetBool("Walking B", false);
            anim.SetBool("Strafe R", false);
            anim.SetBool("Strafe L", false);
        }

        transform.position = playerPosition;
        rb.velocity = new Vector3(0,0,0);   //Freeze velocity
    }


    public void TakeDamage(float damage) {

        health -= damage;

        if (health <= 0) {
            //Disable irrelvant animation bools
            anim.SetBool("Dead", true);
            anim.SetBool("Shooting", false);
            muzzleFlash.SetActive(false);
            anim.SetBool("Flamer", false);
            flameStream.GetComponent<ParticleSystem>().Stop();
            anim.SetBool("Walking F", false);
            anim.SetBool("Walking B", false);
            anim.SetBool("Strafe R", false);
            anim.SetBool("Strafe L", false);
            GameManager.instance.playerDead = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    
    int RandomInt()
    {
        int RandomNumber = Random.Range(randomJamMin, randomJamMax);
        //int RandomNumber = Random.RandomRange();

        return RandomNumber;
    }

    void WeaponJam()
    {
        jamIndicator.SetActive(true);
        if (jamTime <= 0)
        {
            //Debug.Log("this Triggered");
            audioManager.Play("Jam");
            isJammed = false;
            bulletsUntilJam = RandomInt();
            jamTime = 4;
            jamIndicator.SetActive(false);
        }


        if (isJammed == true) // Input.GetMouseButton(0) &&
        {
            jamTime -= Time.deltaTime;
        }

    }

    //End of Level Goal Interaction
    public void OnTriggerEnter(Collider other) {

        if (other.tag == "Goal") {
            GameManager.instance.levelComplete = true;
            StartCoroutine(GameManager.instance.LoadLevel(GameManager.instance.nextLevel));
        }
    }
}
