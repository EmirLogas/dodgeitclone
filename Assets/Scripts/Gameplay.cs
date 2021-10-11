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
    private bool isDead,inMenu;
    public AudioSource aSou;
    public AudioClip clip1, clip2, clip3, clip4, clip5;
    private bool played1 = false, played2 = false, played3 = false, played4 = false;

    public GameObject menuC;
    public GameObject Settings;
    public GameObject MainButtons;
    public Slider volumeSlider;
    public Text volumeValuetxt;
    public GameObject Market;

    public GameObject startButton,resumeButton;


    private void Awake()
    {
        aSou.Stop();
    }
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        IsFrozen = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        inMenu = false;

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
        for (int i = 0; i < 2; i++)
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
        scoreControl();

    }
    public void scoreControl()
    {
        if (score >= 0 && score <= 10)
        {
            zeroTenSong();
        }
        if (score >= 10 && score <= 20 && played1 == false)
        {
            tenTwentySong();
        }
        else if (score >= 20 && score <= 30 && played2 == false)
        {
            twentyThirtySong();
        }
        else if (score >= 30 && score <= 40 && played3 == false)
        {
            thirtyFourtySong();
        }
        else if (score >= 40 && score <= 50 && played4 == false)
        {
            fourtyFiftySong();
        }
    }
    // Song play ----------------------------------------
    public void zeroTenSong()
    {
        aSou.clip = clip1;
        aSou.Play();
    }
    public void tenTwentySong()
    {
        aSou.Stop();
        aSou.clip = clip2;
        aSou.Play();
        played1 = true;
    }
    public void twentyThirtySong()
    {
        aSou.Stop();
        aSou.clip = clip3;
        aSou.Play();
        played2 = true;
    }
    public void thirtyFourtySong()
    {
        aSou.Stop();
        aSou.clip = clip4;
        aSou.Play();
        played3 = true;
    }
    public void fourtyFiftySong()
    {
        aSou.Stop();
        aSou.clip = clip5;
        aSou.Play();
        played4 = true;
    }
    //--------------------------------------------------
    public void Update()
    {
        //Using for when start game and click "w" unfrozen the player.
        if (IsFrozen == true && Input.GetKeyDown(KeyCode.W) && isDead == false && inMenu==false)
        {
            scoreControl();
            pressWtext.gameObject.SetActive(false);
            UnFrozePlayer();
        }
        //Using for tests.
        if (Input.GetKeyDown(KeyCode.H))
        {
            score++;
            txt.text = score.ToString();
            scoreControl();
            for (int i = 0; i < 2; i++)
            {
                GameObject b = Instantiate(redPrefab) as GameObject;
                b.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));
            }

        }
        if (isDead == true)
        {
            aSou.Stop();
            menuC.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
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
        aSou.UnPause();
    }
    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    public void OpenSettings()
    {
        MainButtons.SetActive(false);
        Settings.SetActive(true);
    }
    public void CloseSettings()
    {
        MainButtons.SetActive(true);
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
        volumeValuetxt.text = (volumeSlider.value * 100).ToString();
    }
    public void OpenMarket()
    {
        MainButtons.SetActive(false);
        Market.SetActive(true);
    }
    public void CloseMarket()
    {
        MainButtons.SetActive(true);
        Market.SetActive(false);
    }
    public void Pause()
    {
        FrozePlayer();
        aSou.Pause();
        menuC.SetActive(true);
        startButton.SetActive(false);
        resumeButton.SetActive(true);
        pressWtext.gameObject.SetActive(true);
        inMenu =true;
    }
    public void Resume()
    {
        menuC.SetActive(false);
        startButton.SetActive(true);
        resumeButton.SetActive(false);
        inMenu = false;
    }
}
