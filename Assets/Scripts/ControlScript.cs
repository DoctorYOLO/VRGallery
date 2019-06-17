using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.XR;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using DoubleLinkedList;

public class ControlScript : MonoBehaviour
{
    [Header("Wall Pictures")]
    public GameObject[] spawnPoints;
    public GameObject saveManager;

    [Header("UI")]
    public GameObject menu;
    public int visitorSpeed;

    private bool isPaused = false;
    private bool isPauseIn = false;
    private bool isReturnIn = false;
    private bool isNextIn = false;
    private bool isBackIn = false;

    private static DLOperator dlOperator;
    private static FileInfo[] files;

    // Start is called before the first frame update
    void Start()
    {
        XRSettings.enabled = true;
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/");
        files = di.GetFiles().Where(o => o.Name.EndsWith(".json")).ToArray();

        dlOperator = new DLOperator(files.Length, 2);
        PicturesDisplay(new int[] { 0, 1, 2 });
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");


        if (!isPaused)
        {
            Vector3 movement = Camera.main.transform.TransformDirection(new Vector3(hAxis, 0, vAxis) * visitorSpeed * Time.deltaTime);
            transform.position = transform.position + movement;
        }

        if (Input.GetButtonDown("Submit") && (isPauseIn))
        {
            PauseClick();
        }

        if (Input.GetButtonDown("Submit") && (isReturnIn))
        {
            ReturnToMenuClick();
        }

        if (Input.GetButtonDown("Submit") && (isNextIn))
        {
            NextClick();
        }

        if (Input.GetButtonDown("Submit") && (isBackIn))
        {
            BackClick();
        }
    }

    public async void PicturesDisplay (int[] arr)
    {
        if (files.Length == 1)
        {
            spawnPoints[1].GetComponent<Image>().sprite = await saveManager.GetComponent<SaveLoad>().Load(files[arr[0]].Name);
        }
        if (files.Length == 2)
        {
            spawnPoints[0].GetComponent<Image>().sprite = await saveManager.GetComponent<SaveLoad>().Load(files[arr[0]].Name);
            spawnPoints[1].GetComponent<Image>().sprite = await saveManager.GetComponent<SaveLoad>().Load(files[arr[1]].Name);
        }
        if (files.Length >= 3)
        {
            spawnPoints[0].GetComponent<Image>().sprite = await saveManager.GetComponent<SaveLoad>().Load(files[arr[0]].Name);
            spawnPoints[1].GetComponent<Image>().sprite = await saveManager.GetComponent<SaveLoad>().Load(files[arr[1]].Name);
            spawnPoints[2].GetComponent<Image>().sprite = await saveManager.GetComponent<SaveLoad>().Load(files[arr[2]].Name);
        }
    }

    public void NextClick ()
    {
        if (files.Length > 3)
        {
            int[] arr = dlOperator.PickNext();
            PicturesDisplay(arr);
        }
    }

    public void BackClick ()
    {
        if (files.Length > 3)
        {
            int[] arr = dlOperator.PickPrevious();
            PicturesDisplay(arr);
        }
    }

    public void PauseClick ()
    {
        if (isPaused)
        {
            menu.gameObject.SetActive(false);
            isPaused = false;
        }
        else
        {
            isPaused = true;
            menu.gameObject.SetActive(true);
        }
    }

    public void PauseIn ()
    {
        isPauseIn = true;
    }

    public void PauseOut ()
    {
        isPauseIn = false;
    }

    public void ReturnIn()
    {
        isReturnIn = true;
    }

    public void ReturnOut()
    {
        isReturnIn = false;
    }

    public void NextIn()
    {
        isNextIn = true;
    }

    public void NextOut()
    {
        isNextIn = false;
    }

    public void BackIn()
    {
        isBackIn = true;
    }

    public void BackOut()
    {
        isBackIn = false;
    }

    public void ReturnToMenuClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
