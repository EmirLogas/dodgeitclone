using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float forcePowerUp = 150;
    public float forcePowerHorizontal = 500;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, forcePowerUp));
        }
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-forcePowerHorizontal, 0.0f) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(forcePowerHorizontal, 0.0f) * Time.deltaTime);
        }
    }
}
