using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;
    private static GameObject _player;
    private static PlayerController _playerController;
    private static GameObject _healthController;
    private static TextMeshProUGUI _healthText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        if (_player != null)
        {
            _playerController = _player.GetComponent<PlayerController>();
            _healthController = gameObject.transform.Find("Health").gameObject;
            _healthText = _healthController.GetComponent<TextMeshProUGUI>();
            int health = _playerController.GetHealth();
            SetHealthText(health);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void SetHealthText(int health)
    {
        string healthText = "";
        for (int i = 0; i < health; i++)
        {
            healthText += "<sprite name=\"heart\"> ";
        }
        _healthText.text = healthText;
    }
}
