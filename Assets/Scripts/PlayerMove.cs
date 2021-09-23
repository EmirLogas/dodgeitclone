using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float forcePowerUp = 4;
    public float forcePowerHorizontal = 0.15f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, forcePowerUp) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-forcePowerHorizontal, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(forcePowerHorizontal, 0) * Time.deltaTime);
        }
    }
}
