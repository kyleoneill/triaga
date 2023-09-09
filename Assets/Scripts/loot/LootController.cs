using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour
{
    [SerializeField] private float DespawnTime = 10f;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = gameObject.GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(FlickerSprite), DespawnTime, 0.25f);
        Invoke(nameof(Despawn), DespawnTime + 5f);
    }

    void FlickerSprite()
    {
        _renderer.enabled = !_renderer.enabled;
    }

    public void Despawn()
    {
        CancelInvoke();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
