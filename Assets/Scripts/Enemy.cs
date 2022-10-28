using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    NavMeshAgent agent;

    private AudioManager audioManager;

    public GameObject player;

    public float health = 10.0f;

    public float agroRange = 10.0f;
    public float damage = 5.0f;

    //Rotation vars
    public float rotationSpeed;
    private float adjRotSpeed;
    public Quaternion targetRotation;

    //Laser Damage
    public GameObject laser;
    public GameObject laserMuzzle;
    private float laserTimer;
    public float laserTime = 1.0f;

    //Collision Damage
    private float damageTimer;
    private float damageTime = 0.5f;

    public GameObject burning;
    public GameObject explosion;

    //Turn off firing for tutorial purposes
    public bool armed = true;

    //set melee true for cubes
    public bool melee = false;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();

        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update() {

        Behaviour();

        //Kill check - moved from takeDamage due to bug
        if (health <= 0) {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

    void Behaviour() {

        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");
        else if (player && !GameManager.instance.playerDead) {

            //Raycast in direction of Player
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -(transform.position - player.transform.position).normalized, out hit, agroRange)) {

                //If Raycast hits player
                if (hit.transform.tag == "Player") {

                    Debug.DrawLine(transform.position, player.transform.position, Color.red);

                    //Rotate slowly towards player
                    targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                    adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);

                    //audioManager.Play("RobotAlert");


                    //Move towards player
                    if (Vector3.Distance(player.transform.position, transform.position) >= 5 || melee == true) {
                        agent.SetDestination(player.transform.position);
                    }
                    //Stop if close to player
                    else if (Vector3.Distance(player.transform.position, transform.position) < 5) {
                        agent.SetDestination(transform.position);
                    }

                    //Fire Laser
                    if (Time.time > laserTimer && armed) {
                        Instantiate(laser, laserMuzzle.transform.position, laserMuzzle.transform.rotation);
                        laserTimer = Time.time + laserTime;
                    }
                }
            }
        }
    }


    private void OnCollisionStay(Collision collision) {

        if (collision.transform.tag == "Player" && Time.time > damageTimer) {
            collision.transform.GetComponent<PlayerAvatar>().TakeDamage(damage);
            damageTimer = Time.time + damageTime;
        }
    }

    public void TakeDamage(float thisDamage) {

        health -= thisDamage;
    }

}
