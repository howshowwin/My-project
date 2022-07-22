using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drug : MonoBehaviour
{
    public GameObject selectedObject;

    Vector3 offset;


    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject)
            {
                selectedObject = targetObject.transform.gameObject;
                selectedObject.GetComponent<Renderer>().material.color = Color.blue;
                offset = selectedObject.transform.position - mousePosition;
            }
        }
        if (selectedObject)
        {
            selectedObject.transform.position = mousePosition + offset;

        }
        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject.GetComponent<Renderer>().material.color = Color.yellow;
            selectedObject = null;

        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject);
        other.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        if (other.gameObject.tag == "Square")
        {
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        other.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        selectedObject.GetComponent<Renderer>().material.color = Color.blue;
        if (other.gameObject.tag == "Square")
        {
        }
    }
}
