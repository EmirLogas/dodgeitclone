using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    public GameObject blackPrefab, redPrefab;
    private Vector2 screenBounds;
    public int redCount = 1, score = 0;
    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        GameObject a = Instantiate(blackPrefab) as GameObject;
        a.transform.position = new Vector2(Random.Range(-screenBounds.x,screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));
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
        SpawnObject(redCount);
    }
}
