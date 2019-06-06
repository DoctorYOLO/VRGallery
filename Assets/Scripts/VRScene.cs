using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VRScene : MonoBehaviour
{
    public GameObject[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        var saveManager = GameObject.FindGameObjectWithTag("SaveManager");
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/");
        var files = di.GetFiles().Where(o => o.Name.EndsWith(".json")).ToArray();
        spawnPoints[0].GetComponent<Image>().sprite = saveManager.GetComponent<SaveLoad>().Load(files[0].Name);
        spawnPoints[1].GetComponent<Image>().sprite = saveManager.GetComponent<SaveLoad>().Load(files[1].Name);
        spawnPoints[2].GetComponent<Image>().sprite = saveManager.GetComponent<SaveLoad>().Load(files[2].Name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
