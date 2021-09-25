using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    public GameObject blackPrefab, redPrefab;
    private Vector2 screenBounds;
    public int redCount = 1, score = 0;
    public Text txt;
    Rigidbody2D rb;
    public bool IsFrozen2;
    public GameObject pressWtext;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        GameObject a = Instantiate(blackPrefab) as GameObject;
        a.transform.position = new Vector2(Random.Range(-screenBounds.x,screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));
        GameObject b = Instantiate(redPrefab) as GameObject;
        b.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));
        bool IsFrozen = (rb.constraints & RigidbodyConstraints2D.FreezePosition) == RigidbodyConstraints2D.FreezePosition;
        IsFrozen2 = IsFrozen;
    }
    void SpawnObject(int redCount)
    {
        GameObject a = Instantiate(blackPrefab) as GameObject;
        a.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));
        for (int i = 0; i < redCount; i++)
        {
            GameObject b = Instantiate(redPrefab) as GameObject;
            b.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));
        }
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Circle (1)"))
        {
            Debug.Log("kek");
            Application.Quit();
        }
        Destroy(other.gameObject);
        redCount += 2;
        score++;
        txt.text = score.ToString();
        
        SpawnObject(redCount);
    }

    public void Update()
    {
        

        if (IsFrozen2==true && Input.GetKeyDown("w"))
        {
            IsFrozen2 = false;
            rb.constraints = RigidbodyConstraints2D.None;
            pressWtext.gameObject.SetActive(false);
        }
    }
}
