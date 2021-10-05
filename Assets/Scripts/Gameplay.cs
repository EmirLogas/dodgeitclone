using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gameplay : MonoBehaviour
{
    public GameObject blackPrefab, redPrefab;
    private Vector2 screenBounds;
    private int score;
    public Text txt;
    Rigidbody2D rb;
    private bool IsFrozen;
    public GameObject pressWtext;
    private bool isDead;
    public AudioSource aSou;
    public AudioClip clip1, clip2, clip3, clip4, clip5;
    private bool played1 = false, played2 = false, played3 = false, played4 = false;

    public GameObject menuC;
    public GameObject Settings;
    public GameObject StartButton, SettingsButton, QuitButton;
    public Slider volumeSlider;


    private void Awake()
    {
        aSou.Stop();
    }
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        IsFrozen = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        score = 0;
        isDead = false;

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        GameObject a = Instantiate(blackPrefab) as GameObject;
        a.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));
        GameObject b = Instantiate(redPrefab) as GameObject;
        b.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));


        if (PlayerPrefs.HasKey("volume"))
        {
            aSou.volume = PlayerPrefs.GetFloat("volume");
            volumeSlider.value = aSou.volume;
        }
    }
    void SpawnObject()
    {
        GameObject a = Instantiate(blackPrefab) as GameObject;
        a.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));
        GameObject b = Instantiate(redPrefab) as GameObject;
        b.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //When touch redDots
        if (other.gameObject.name.Contains("RedDot"))
        {
            isDead = true;
            transform.position = new Vector3(0, 0, 0);
            pressWtext.SetActive(true);

            FrozePlayer();
        }
        //When touch BlackDot
        else if (other.gameObject.name.Contains("BlackDot"))
        {
            Destroy(other.gameObject);
            score++;
            txt.text = score.ToString();
            SpawnObject();
        }
        else if (other.gameObject.name.Contains("TopCollider") || other.gameObject.name.Contains("BottomCollider") || other.gameObject.name.Contains("LeftCollider") || other.gameObject.name.Contains("RightCollider"))
        {
            isDead = true;
            aSou.Stop();
            FrozePlayer();
        }

        if (score >= 10 && score <= 20 && played1 == false)
        {
            aSou.Stop();
            aSou.clip = clip2;
            aSou.Play();
            played1 = true;
        }
        else if (score >= 20 && score <= 30 && played2 == false)
        {
            aSou.Stop();
            aSou.clip = clip3;
            aSou.Play();
            played2 = true;
        }
        else if (score >= 30 && score <= 40 && played3 == false)
        {
            aSou.Stop();
            aSou.clip = clip4;
            aSou.Play();
            played3 = true;
        }
        else if (score >= 40 && score <= 50 && played4 == false)
        {
            aSou.Stop();
            aSou.clip = clip5;
            aSou.Play();
            played4 = true;
        }
    }

    public void Update()
    {
        //Using for when start game and click "w" unfrozen the player.
        if (IsFrozen == true && Input.GetKeyDown(KeyCode.W) && isDead == false)
        {
            aSou.clip = clip1;
            aSou.Play();
            UnFrozePlayer();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            score++;
            txt.text = score.ToString();
        }
        if (isDead == true)
        {
            aSou.Stop();
            menuC.SetActive(true);
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
    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    public void OpenSettings()
    {
        StartButton.SetActive(false);
        SettingsButton.SetActive(false);
        QuitButton.SetActive(false);
        Settings.SetActive(true);
    }
    public void CloseSettings()
    {
        StartButton.SetActive(true);
        SettingsButton.SetActive(true);
        QuitButton.SetActive(true);
        Settings.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void VolumeChange()
    {
        aSou.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }

}
