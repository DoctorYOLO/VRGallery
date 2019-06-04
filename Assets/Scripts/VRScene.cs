using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRScene : MonoBehaviour
{
    public GameObject[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints[0].GetComponent<Image>().sprite = Store.spritesList[0];
        spawnPoints[1].GetComponent<Image>().sprite = Store.spritesList[1];
        spawnPoints[2].GetComponent<Image>().sprite = Store.spritesList[2];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
