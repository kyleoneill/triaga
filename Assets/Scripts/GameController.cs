using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        NotPlaying,
        Alive,
        Dead
    }
    public static GameController Instance;
    private static int _currentScene;
    private SceneController _sceneController;
    [SerializeField] private GameObject playerPrefab;
    public GameObject player;
    public PlayerController playerController;
    private CameraController _cameraController;
    private GameState _gameState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            setPlayerState(GameState.NotPlaying);
            
            // Debug, do scene setup if we begin a non-main-menu scene in the editor
            #if UNITY_EDITOR
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                _currentScene = SceneManager.GetActiveScene().buildIndex;
                StartScene();
            }
            #endif
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
        if (_gameState == GameState.Dead && Input.GetKeyDown(KeyCode.R))
        {
            TransitionScene(0);
        }
    }

    public void TransitionScene(int sceneNumber)
    {
        if (sceneNumber == 0)
        {
            setPlayerState(GameState.NotPlaying);
            _currentScene = 0;
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(sceneNumber);
        
            // Start a coroutine so the scene instantiation logic isn't called until the scene is done loading
            if (SceneManager.GetActiveScene().buildIndex != sceneNumber)
            {
                StartCoroutine("waitForSceneLoad", sceneNumber);
            }   
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
            StartScene();
        }
    }

    private void StartScene()
    {
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
        setPlayerState(GameState.Alive);
        _sceneController = GameObject.FindWithTag("SceneController").GetComponent<SceneController>();
        _sceneController.StartScene();
    }

    public void setPlayerState(GameState newState)
    {
        _gameState = newState;
        if (_gameState == GameState.Dead)
        {
            _sceneController.StopScene();
        }
    }
}
