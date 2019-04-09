using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRScene : MonoBehaviour
{
    public GameObject spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint.GetComponent<SpriteRenderer>().sprite = Store.vrPicture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
