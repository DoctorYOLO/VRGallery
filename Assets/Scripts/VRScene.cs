using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRScene : MonoBehaviour
{
    public GameObject spawnPoint;

    private string pictureName = Store.pictureName;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Pictures/" + pictureName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
