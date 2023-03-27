using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSW : MonoBehaviour
{
    [Header("Variáveis")]
    public float health; 
    [SerializeField] float speed;
    [SerializeField] float horizontalLimit; 
    [SerializeField] bool shoot; 
    float shootTimer; 
    int projectileIndex; 

    [Header("Componentes")]
    [SerializeField] GameObject[] projectilePrefab; 
    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        projectileIndex = 0; 
        shootTimer = projectilePrefab[projectileIndex].GetComponent<projectileSW>().shootCooldown;
        health = 100f; 
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody>().velocity = new Vector3(speed*x,0f,0f);
        if(x>0 && transform.position.x>=horizontalLimit) GetComponent<Rigidbody>().velocity = Vector3.zero; 
        if(x<0 && transform.position.x<=-horizontalLimit) GetComponent<Rigidbody>().velocity = Vector3.zero;

        if(shoot)
        {
            if(Input.GetAxis("Fire1") > 0 && shootTimer <  0)
            {
                GameObject projectileInstance = Instantiate(projectilePrefab[projectileIndex], transform);
                projectileInstance.transform.SetParent(this.transform);
                projectileInstance.transform.position = transform.position;
                shootTimer = projectilePrefab[projectileIndex].GetComponent<projectileSW>().shootCooldown;
            }
            if(Input.GetKeyDown(KeyCode.Space))
            {
                projectileIndex++;
                if(projectileIndex == projectilePrefab.Length)
                    projectileIndex = 0; 
            }
            shootTimer -= Time.deltaTime; 
        }

        if(health <=0f)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.parent);
            explosion.transform.position = transform.position;
            Destroy(this.gameObject);
            Debug.Log("O jogo acabou");
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage; 
        if(health>100f) health = 100f; 
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EnemyProjectile")
        {
            TakeDamage(other.gameObject.GetComponent<projectileSW>().damage);
            Destroy(other.gameObject);
            GameObject hit = Instantiate(hitEffect,transform);
            hit.transform.position = transform.position; 
        }

        if(other.tag == "Suply") 
        {
            TakeDamage(-25f);
            Destroy(other.gameObject);
        }
    }
}
