using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScript : MonoBehaviour
{

    public int visitorSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveForward ()
    {
        transform.position = transform.position + Camera.main.transform.forward * visitorSpeed * Time.deltaTime;
    }

    public void MoveBackward ()
    {
        transform.position = transform.position - Camera.main.transform.forward * visitorSpeed * Time.deltaTime;
    }
}
