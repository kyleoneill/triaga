using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 position = player.transform.position;
        position = new Vector3(position.x, position.y + 8.5f, -10f);
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
