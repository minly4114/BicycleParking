using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour {

    public enum States {Up, Down, Right, Left, Forvard, Back, Idle };
    public States State = States.Idle;

    public float ElevatorVerticalSpeed = 1.0f;
    public float ElevatorRotationSpeed = 0.5f;

    //public GameObject Elevator;
    public GameObject BicyclesArray;

    private GameObject _bicycle;

    private void FixedUpdate()
    {
        DoAction();
    }

    private void DoAction()
    {
        Vector3 angles = transform.rotation.eulerAngles;
        switch (State)
        {
            case States.Up:
                transform.position += Vector3.up * Time.deltaTime * ElevatorVerticalSpeed;
                break;
            case States.Down:
                transform.position += Vector3.down * Time.deltaTime * ElevatorVerticalSpeed;
                break;
            case States.Right:               
                transform.rotation = Quaternion.Euler(angles.x, angles.y+ElevatorRotationSpeed, angles.z);
                break;
            case States.Left:
                transform.rotation = Quaternion.Euler(angles.x, angles.y - ElevatorRotationSpeed, angles.z);
                break;
            case States.Forvard:
                transform.position += transform.forward * Time.deltaTime * ElevatorVerticalSpeed;
                break;
            case States.Back:
                transform.position += -transform.forward * Time.deltaTime * ElevatorVerticalSpeed;
                break;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag =="Bicycle")
        {
            Debug.Log(other.transform.position);
            AttachBicycle(other.gameObject);
            Debug.Log(other.transform.position);
        }
        
    }

    public void AttachBicycle(GameObject bicycle)
    {
        if (_bicycle == null)
        {
            _bicycle = bicycle;
            _bicycle.transform.parent = transform;
            bicycle.transform.position = new Vector3(transform.position.x , transform.position.y , transform.position.z );
        }
    }
    public void DeattachBicycle()
    {
        _bicycle.transform.parent = BicyclesArray.transform;
    }
}
