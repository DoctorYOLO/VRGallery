using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ControlScript : MonoBehaviour
{

    public GameObject Menu;
    public int visitorSpeed;

    private bool isMoving = false;
    private bool moveForward = true;
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (isMoving)
        {
            if (moveForward)
            {
                transform.position = transform.position + Camera.main.transform.forward * visitorSpeed * Time.deltaTime;
            }
            if (!moveForward)
            {
                transform.position = transform.position - Camera.main.transform.forward * visitorSpeed * Time.deltaTime;
            }
        }
        */
        if (Input.GetButton("Fire1") && (!isPaused))
        {
            transform.position = transform.position + Camera.main.transform.forward * visitorSpeed * Time.deltaTime;
        }
    }

    public void PauseClick ()
    {
        if (isPaused)
        {
            Menu.gameObject.SetActive(false);
            isPaused = false;
        }
        else
        {
            isPaused = true;
            Menu.gameObject.SetActive(true);
        }
    }

    public void ReturnToMenuClick ()
    {
        Store.vrPicture = null;
        SceneManager.LoadScene("Menu");
    }

    /*
    public void MoveForwardUp ()
    {
        isMoving = false;
    }

    public void MoveForwardDown ()
    {
        isMoving = true;
        moveForward = true;
    }

    public void MoveBackwardUp ()
    {
        isMoving = false;
    }

    public void MoveBackwardDown ()
    {
        isMoving = true;
        moveForward = false;
    }
    */
}
