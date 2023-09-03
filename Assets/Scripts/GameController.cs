using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private static int _currentScene;
    private SceneController _sceneController;
    [SerializeField] private GameObject playerPrefab;
    public GameObject player;
    public PlayerController playerController;
    private CameraController _cameraController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Run scene instantiation logic
            TransitionScene(0);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TransitionScene(int sceneNumber)
    {
        // Don't do anything if we are at the main menu
        if (sceneNumber == 0)
            return;
        
        SceneManager.LoadScene(sceneNumber);
        
        // Start a coroutine so the scene instantiation logic isn't called until the scene is done loading
        if (SceneManager.GetActiveScene().buildIndex != sceneNumber)
        {
            StartCoroutine("waitForSceneLoad", sceneNumber);
        }
    }

    IEnumerator waitForSceneLoad(int sceneNumber)
    {
        while (SceneManager.GetActiveScene().buildIndex != sceneNumber)
        {
            yield return null;
        }

        if (SceneManager.GetActiveScene().buildIndex == sceneNumber)
        {
            _currentScene = sceneNumber;
            
            // Find the player spawner and spawn the player
            GameObject playerSpawner = GameObject.FindWithTag("PlayerSpawner");
            player = Instantiate(playerPrefab);
            player.transform.position = playerSpawner.transform.position;
            playerController = player.GetComponent<PlayerController>();

            // After the player is spawned, set up the camera
            Camera camera = Camera.main;
            _cameraController = camera.GetComponent<CameraController>();
            _cameraController.InstantiateCamera();
            
            // After the player and camera are set up, begin the scene
            _sceneController = GameObject.FindWithTag("SceneController").GetComponent<SceneController>();
            _sceneController.StartScene();
        }
    }
}
