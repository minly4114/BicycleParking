using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandMoverController : MonoBehaviour {

    public GameObject BlackBox;
    private GameObject _bicycle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bicycle")
        {
            Debug.Log(other.transform.position);
            BlackBox.GetComponent<ElevatorController>().AttachBicycle(other.gameObject);
            Debug.Log(other.transform.position);
        } 
    }
}
