using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySW : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] GameObject explosionEffect; 
    [SerializeField] GameObject hitEffect; 
    [SerializeField] GameObject suplyEffect;

    [Header("Variáveis")]
    [SerializeField] float horizontalSpeed; 
    [SerializeField] float verticalSpeed; 
    [SerializeField] float horizontalLimit;
    float verticalLimit = -6.4f; 
    [SerializeField] float offset; 
    [SerializeField] projectileSW projectile;
    float shootTimer; 
    float constHorizontalSpeed;  

    [Header("Parametros")]
    [SerializeField] float health;
    [SerializeField] int points; 

    [Header("Caracteristicas")]
    [SerializeField] bool shoot;
    [SerializeField] bool kamikase;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(horizontalSpeed, -verticalSpeed, 0f);
        if(shoot)
            shootTimer = projectile.shootCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        //if follow is true
        if(GameManagerSW.instance.isRunning)
        {
            //Para o Kamikase: seguir o personagem
            if(GameManagerSW.instance.player != null && kamikase)
            {
                if(GameManagerSW.instance.player.transform.position.x == transform.position.x)
                    horizontalSpeed = 0f; 
                else if(GameManagerSW.instance.player.transform.position.x > transform.position.x)
                    horizontalSpeed = 1f;
                else if(GameManagerSW.instance.player.transform.position.x < transform.position.x)
                    horizontalSpeed = -1f; 
                GetComponent<Rigidbody>().velocity = new Vector3(horizontalSpeed, -verticalSpeed, 0f);
            } 
            else //Para os demais: andar de um lado para o outro
            {
                if((transform.position.x > horizontalLimit && horizontalSpeed > 0) || (transform.position.x < -horizontalLimit && horizontalSpeed < 0)) 
                {
                    horizontalSpeed *=-1;
                    GetComponent<Rigidbody>().velocity = new Vector3(horizontalSpeed, - verticalSpeed, 0f);
                } 
            }
            
            //Configuração do tiro
            if(shoot)
            {
                if(shootTimer <=0)
                {
                    if(Random.Range(0f,100f)>60f)
                    {
                        GameObject projectileInstance = Instantiate(projectile.gameObject, transform);
                        projectileInstance.transform.position = transform.position;
                        projectileInstance.transform.SetParent(this.transform.parent);

                        projectileInstance.GetComponent<projectileSW>().speed*=-1; 
                    }
                    shootTimer = projectile.GetComponent<projectileSW>().shootCooldown; 
                }
                shootTimer-=Time.deltaTime;

            }
            
            //Quando a vida do inimigo fica sem vida
            if(health <= 0)
            {
                bool spawnSuply = GameManagerSW.instance.AddPoints(points);
                GameObject explosion = Instantiate(explosionEffect, transform.parent);
                explosion.transform.position = transform.position;
                if(spawnSuply)
                {
                    GameObject suply = Instantiate(suplyEffect, transform.parent);
                    suply.transform.position = transform.position;
                }
                Destroy(explosion, 3);
                Destroy(this.gameObject);
            }

            //quando o inimigo chegar no limite inferior da tela
            if(transform.position.y < verticalLimit)
            {
                Destroy(this.gameObject);
                Destroy(GameManagerSW.instance.player.gameObject);
            }
        }
    }

//Função que tira vida do inimigo
    void TakeDamage(float damage)
    {
        health -= damage; 
    }

    void OnTriggerEnter(Collider other)
    {
        //Contato com o Projétil
        if(other.tag == "Projectile")
        {
            TakeDamage(other.gameObject.GetComponent<projectileSW>().damage);
            if(health>0)
            {
               GameObject hit = Instantiate(hitEffect, transform.parent);
               hit.transform.position = transform.position; 
               Destroy(hit, 3);
            }
            Destroy(other.gameObject);
        }
        //contato com o jogador
        else if(other.tag == "Player")
        {
            if(!kamikase) //se não for kamikase: gameover
            {
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
            else //kamikase tira vida do jogador
            {
                GameManagerSW.instance.player.TakeDamage(health);
                Destroy(this.gameObject);
            }     
            GameObject explosion = Instantiate(explosionEffect, transform.parent);
            explosion.transform.position = transform.position;
            Destroy(explosion, 3);
        }  
        //contato entre inimigos
        else if(other.tag == "EnemySW" && !kamikase)
        {
            horizontalSpeed *= -1;
            GetComponent<Rigidbody>().velocity = new Vector3(horizontalSpeed, -verticalSpeed, 0f);
        }
    }
}