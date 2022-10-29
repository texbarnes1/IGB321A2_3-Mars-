using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] public float health = 10000f;
    [SerializeField] private float agroRange = 10.0f;
    [SerializeField] private float damage = 5.0f;
    [SerializeField] private float rotationSpeed;
    
    [SerializeField] private GameObject smallEnemyPrefab;
    [SerializeField] private GameObject largeEnemyPrefab;
    [SerializeField] private GameObject dronepPrefab;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Transform spawnersParent;
    private List<Transform> spawners = new List<Transform>();
    [SerializeField] private Transform muzzle;

    private GameObject player;

    private float damageTimer;
    [SerializeField] private float damageTime = 0.5f;

    private float spawnTimer;
    [SerializeField] private float spawnTime = 2f;

    private float shootTimer;
    [SerializeField] private float shootTime = 2.5f;

    private bool isAggro = false;

    private int stage = 1;
    private List<Enemy> children = new List<Enemy>(); //an array to hold all children so they explode with the boss
    private void Start()
    {
        foreach (Transform transform in spawnersParent)
        {
            spawners.Add(transform);
        }
    }

    private void Update()
    {
        LookAtPlayer();
        //Debug.Log(isAggro);
        if (isAggro) SpawnSmallEnemies();

        RotateSpawners();

        if (isAggro) Shoot();

        //Kill check - moved from takeDamage due to bug
        if (health <= 0)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            foreach (Enemy enemy in children)
            {
                enemy.health = 0;
            }
            GameManager.instance.levelComplete = true;
            Destroy(gameObject);
        }
        else if (health < 4000 && stage == 1)
        {
            SpawnDrones(1);
            stage = 2;
        }
        else if (health < 1000 && stage == 2)
        {
            SpawnDrones(2);
            stage = 3;
        }
    }

    private void LookAtPlayer()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");
        else if (player && !GameManager.instance.playerDead)
        {
            //Raycast in direction of Player
            if (Physics.Raycast(transform.position, -(transform.position - player.transform.position).normalized, out RaycastHit hit, agroRange))
            {
                //If Raycast hits player
                if (hit.transform.CompareTag("Player"))
                {
                    isAggro = true;
                    Debug.DrawLine(transform.position, player.transform.position, Color.red);

                    //Rotate slowly towards player
                    Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                    float adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);
                }
                else
                {
                    isAggro = false;
                }
            }
        }
    }

    private void SpawnSmallEnemies()
    {
        GameObject child;
        if (Time.time > spawnTimer)
        {
            foreach (Transform transform in spawners)
            {
                if (Random.Range(1,6) == 1) //1 in 6 chance to spawn larger enemy
                {
                    child = Instantiate(largeEnemyPrefab, transform.position, transform.rotation);
                }
                else
                {
                    child = Instantiate(smallEnemyPrefab, transform.position, transform.rotation);
                }
                children.Add(child.GetComponent<Enemy>()); 
            }
            spawnTimer = Time.time + spawnTime;
        }
    }
    private void SpawnDrones(int count)
    {
        GameObject child;
        for (int i = 1; i <= count; i++)
        {
            Transform transform = spawners[Random.Range(0, spawners.Count)];
            child = Instantiate(dronepPrefab, transform.position, transform.rotation);
            children.Add(child.GetComponent<Enemy>());
        }
    }

    private void RotateSpawners()
    {
        spawnersParent.Rotate(spawnersParent.position, 2f * Time.deltaTime);
    }

    private void Shoot()
    {
        //Fire 'weapon'
        if (Time.time > shootTimer)
        {

            GameObject child = Instantiate(smallEnemyPrefab, muzzle.position, muzzle.rotation);
            children.Add(child.GetComponent<Enemy>());
            shootTimer = Time.time + shootTime;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && Time.time > damageTimer)
        {
            collision.transform.GetComponent<PlayerAvatar>().TakeDamage(damage);
            damageTimer = Time.time + damageTime;
        }
    }

    public void TakeDamage(float thisDamage)
    {
        health -= thisDamage;
    }
}