using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public static string CurrentScene;
    [SerializeField] private GameObject playerPrefab;
    public GameObject player;
    public PlayerController playerController;
    private static String[] _gameScenes;

    private CameraController _cameraController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            InstantiateSceneCollection();
            
            // Run scene instantiation logic
            TransitionScene();
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

    private void TransitionScene()
    {
        // Check if our current scene is a game scene (Not the main menu, etc)
        CurrentScene = SceneManager.GetActiveScene().name;
        if (_gameScenes.Contains(CurrentScene))
        {
            // If it is, find the player spawner and spawn the player
            GameObject playerSpawner = GameObject.FindWithTag("PlayerSpawner");
            if (playerSpawner == null) return;
            player = Instantiate(playerPrefab);
            player.transform.position = playerSpawner.transform.position;
            playerController = player.GetComponent<PlayerController>();

            // After the player is spawned, set up the camera
            Camera camera = Camera.main;
            if (camera == null) return;
            _cameraController = camera.GetComponent<CameraController>();
            if (_cameraController == null) return;
            _cameraController.InstantiateCamera();
            
        }
    }

    private static void InstantiateSceneCollection()
    {
        // TODO: There _has_ to be a better way to store and reference this information
        _gameScenes = new String[1];
        _gameScenes[0] = "LevelOne";
    }
}
