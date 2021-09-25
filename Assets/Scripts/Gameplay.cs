using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    public GameObject blackPrefab, redPrefab;
    private Vector2 screenBounds;
    public int redCount, score;
    public Text txt;
    Rigidbody2D rb;
    public bool IsFrozen;
    public GameObject pressWtext;
    public bool isDead;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        IsFrozen = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        redCount = 1;
        score = 0;
        isDead = false;

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        GameObject a = Instantiate(blackPrefab) as GameObject;
        a.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));
        GameObject b = Instantiate(redPrefab) as GameObject;
        b.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        //When touch redDots
        if (other.gameObject.name.Contains("RedDot"))
        {
            isDead = true;
            FrozePlayer();
        }
        //When touch BlackDot
        else if (other.gameObject.name.Contains("BlackDot"))
        {
            Destroy(other.gameObject);
            redCount += 2;
            score++;
            txt.text = score.ToString();
            SpawnObject(redCount);
        }
        else if (other.gameObject.name.Contains("TopCollider") || other.gameObject.name.Contains("BottomCollider") || other.gameObject.name.Contains("LeftCollider") || other.gameObject.name.Contains("RightCollider"))
        {
            isDead = true;
            FrozePlayer();
        }
    }

    public void Update()
    {
        //Using for when start game and click "w" unfrozen the player.
        if (IsFrozen == true && Input.GetKeyDown(KeyCode.W) && isDead == false)
        {
            UnFrozePlayer();
        }
    }

    //When you want froze player use this void
    public void FrozePlayer()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        IsFrozen = true;
    }

    //When you want unfroze player use this void
    public void UnFrozePlayer()
    {
        IsFrozen = false;
        rb.constraints = RigidbodyConstraints2D.None;
        pressWtext.gameObject.SetActive(false);
    }
}
