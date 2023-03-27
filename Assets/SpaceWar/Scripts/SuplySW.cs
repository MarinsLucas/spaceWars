using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuplySW : MonoBehaviour
{
    public float verticalSpeed; 
    void Start()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0f, -verticalSpeed, 0f);
    }

    void Update()
    {
        if(transform.position.y <= -6.4f)
            Destroy(this.gameObject);
    }
}