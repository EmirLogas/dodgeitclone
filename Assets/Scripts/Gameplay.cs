using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class Gameplay : MonoBehaviour
{
    // Screen Size
    private Vector2 screenBounds;

    // Player
    private Rigidbody2D rb;
    private bool IsFrozen;

    // Prefabs
    public GameObject blackPrefab, redPrefab;
    public ParticleSystem iceSystem,BlackDotSystem;

    //list is better than array
    //private GameObject[] redPrefabArray = new GameObject[300];// For red prefabs
    List<GameObject> redPrefabList = new List<GameObject>();// For red prefabs

    //public short redPrefabCount = 0; // u can use redPrefabList.count

    // Score
    [SerializeField]
    private int score;
    public Text score_Text;

    // Audio, Song
    public AudioSource aSou;
    public List<AudioClip> clips = new List<AudioClip>();

    // Menu/Pause Menu
    public GameObject menu_Canvas;
    public GameObject mainButtons;
    public GameObject market_Panel;
    public GameObject startButton, resumeButton;
    private bool isDead, inMenu;
    public GameObject pressWtext;

    // Settings
    public GameObject settings_Panel;

    // Settings/Volume
    public Slider volumeSlider;
    public Text volumeValueTxt;
    // Market Power 1 -- İce Box
    public GameObject icePrefab;

    private void Awake()
    {
        // Stop Audio Source before the start
        aSou.Stop();
    }
    void Start()
    {
        // Froze player at start
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        IsFrozen = true;

        // Default variables
        inMenu = false;
        isDead = false;
        score = 0;

        // Set screen size
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // Spawn a RedDot and BlackDot for Start the game
        SpawnObject();

        // Check saved volume settings
        if (PlayerPrefs.HasKey("volume"))
        {
            aSou.volume = PlayerPrefs.GetFloat("volume");
            volumeSlider.value = aSou.volume;
            volumeValueTxt.text = Mathf.Round(volumeSlider.value * 100).ToString();
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
            //--- BlackDot particle spawn
            Vector2 pos =other.gameObject.transform.position;
            ParticleSystem pss = Instantiate(BlackDotSystem);
            pss.transform.position = pos;
            //----

            Destroy(other.gameObject);
            score++;
            score_Text.text = score.ToString();
            SpawnObject();
        }
        else if (other.gameObject.name.Contains("TopCollider") || other.gameObject.name.Contains("BottomCollider") || other.gameObject.name.Contains("LeftCollider") || other.gameObject.name.Contains("RightCollider"))
        {
            isDead = true;
            aSou.Stop();
            FrozePlayer();
        }
        ChangeMusic();
    }
    void SpawnObject()
    {

        GameObject blackPrefab_Instantiate = Instantiate(blackPrefab) as GameObject;
        blackPrefab_Instantiate.transform.position = new Vector2(UnityEngine.Random.Range(-screenBounds.x, screenBounds.x), UnityEngine.Random.Range(-screenBounds.y, screenBounds.y));


        GameObject redPrefab_Instantiate = Instantiate(redPrefab) as GameObject;
        redPrefab_Instantiate.transform.position = new Vector2(UnityEngine.Random.Range(-screenBounds.x, screenBounds.x), UnityEngine.Random.Range(-screenBounds.y, screenBounds.y));
        redPrefabList.Add(redPrefab_Instantiate);
    }
    public void ChangeMusic()
    {
        if (score % 10 == 0)
        {
            aSou.Stop();
        }
        if (score < 10)
        {
            aSou.clip = clips[0];
        }
        else if (score >= 10 && score < 20)
        {
            aSou.clip = clips[1];
        }
        else if (score >= 20 && score < 30)
        {
            aSou.clip = clips[2];
        }
        else if (score >= 30 && score < 40)
        {
            aSou.clip = clips[3];
        }
        else if (score >= 40 && score < 50)
        {
            aSou.clip = clips[4];
        }
        if (score % 10 == 0)
        {
            aSou.Play();
        }
    }
    public void Update()
    {
        //Using for when start game and click "w" unfrozen the player.
        if (IsFrozen == true && Input.GetKeyDown(KeyCode.W) && isDead == false && inMenu == false)
        {
            ChangeMusic();
            pressWtext.gameObject.SetActive(false);
            UnFrozePlayer();
        }
        //Using for tests.
        if (Input.GetKeyDown(KeyCode.H))
        {
            score++;
            score_Text.text = score.ToString();
            ChangeMusic();
            GameObject b = Instantiate(redPrefab) as GameObject;
            b.transform.position = new Vector2(UnityEngine.Random.Range(-screenBounds.x, screenBounds.x), UnityEngine.Random.Range(-screenBounds.y, screenBounds.y));
            redPrefabList.Add(b);
        }
        if (isDead == true)
        {
            aSou.Stop();
            menu_Canvas.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        if (Input.GetKeyDown(KeyCode.Z))// Power 1 , ice the red prefs
        {
            MakeThemUntouchable();
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
        mainButtons.SetActive(false);
        settings_Panel.SetActive(true);
    }
    public void CloseSettings()
    {
        mainButtons.SetActive(true);
        settings_Panel.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void VolumeChange()
    {
        aSou.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        volumeValueTxt.text = Mathf.Round(volumeSlider.value * 100).ToString();
    }
    public void OpenMarket()
    {
        mainButtons.SetActive(false);
        market_Panel.SetActive(true);
    }
    public void CloseMarket()
    {
        mainButtons.SetActive(true);
        market_Panel.SetActive(false);
    }
    public void PauseGame()
    {
        FrozePlayer();
        aSou.Pause();
        menu_Canvas.SetActive(true);
        startButton.SetActive(false);
        resumeButton.SetActive(true);
        pressWtext.gameObject.SetActive(true);
        inMenu = true;
    }
    public void ResumeGame()
    {
        menu_Canvas.SetActive(false);
        startButton.SetActive(true);
        resumeButton.SetActive(false);
        inMenu = false;
    }
    public void MakeThemUntouchable()
    {
        for (int i = 0; i < redPrefabList.Count; i++) // Spawn icepref on top of the redprefs in redPrefabArray. 
        {
            GameObject a = redPrefabList[i];
            a.GetComponent<Collider2D>().enabled = false;
            float x, y;
            x = a.transform.position.x;
            y = a.transform.position.y;
            GameObject k = Instantiate(icePrefab) as GameObject;
            k.transform.position = new Vector2(x, y);
            ParticleSystem ps=Instantiate(iceSystem) as ParticleSystem;
            ps.transform.position=new Vector2(x, y);
        }
    }
}