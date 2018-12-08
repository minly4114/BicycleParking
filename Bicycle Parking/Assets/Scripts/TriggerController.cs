using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour {

    public GameObject BlackBox;

    private void OnTriggerEnter(Collider other)
    {
        BlackBox.GetComponent<ElevatorController>().TriggerEvent(name);
    }
}
