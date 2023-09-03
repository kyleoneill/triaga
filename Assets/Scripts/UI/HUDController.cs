using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;
    private static GameObject _healthController;
    private static TextMeshProUGUI _healthText;
    private static TextMeshProUGUI _rupeeText;
    private static TextMeshProUGUI _scoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _healthText = gameObject.transform.Find("Health").gameObject.GetComponent<TextMeshProUGUI>();
            _rupeeText = gameObject.transform.Find("Rupees").gameObject.GetComponent<TextMeshProUGUI>();
            _scoreText = gameObject.transform.Find("Score").gameObject.GetComponent<TextMeshProUGUI>();
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

    internal void SetRupeeText(int numOfRupees)
    {
        _rupeeText.text = $"<sprite=\"items\" name=\"green-rupee\">   {numOfRupees}";
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

    internal void SetScoreText(int score)
    {
        _scoreText.text = $"Score: {score}";
    }
}
