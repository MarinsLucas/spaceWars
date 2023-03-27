using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileSW : MonoBehaviour
{
    public float speed; 
    [SerializeField] float verticalLimit; 
    public float damage; 
    public float shootCooldown; 
    void Start()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0f,speed, 0f);   
    }
    void Update()
    {
        if(transform.position.y > verticalLimit || transform.position.y < -verticalLimit)
            Destroy(this.gameObject);
    }
}
