using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour {

    public enum States {Up, Down, Right, Left, Forvard, Back, Idle };
    public States State = States.Idle;

    public float ElevatorVerticalSpeed = 1.0f;
    public float ElevatorRotationSpeed = 0.5f;

    public GameObject BicyclesArray;
    public GameObject HandMover;
    public GameObject RingMover;
    public GameObject PlaceHandle;
    public GameObject Hand1;
    public GameObject Han2;

    private GameObject _bicycle;

    private void FixedUpdate()
    {
        DoAction();
    }

    private void DoAction()
    {
        Vector3 angles = RingMover.transform.rotation.eulerAngles;
        switch (State)
        {
            case States.Up:
                Up();
                break;
            case States.Down:
                Down();
                break;
            case States.Right:
                Right();
                break;
            case States.Left:
                Left();
                break;
            case States.Forvard:
                Forward();
                break;
            case States.Back:
                Back();
                break;
        }
    }
    private void Up()
    {
        PlaceHandle.transform.position += Vector3.up * Time.deltaTime * ElevatorVerticalSpeed;
    }
    private void Down()
    {
        PlaceHandle.transform.position += Vector3.down * Time.deltaTime * ElevatorVerticalSpeed;
    }
    private void Right()
    {
        Vector3 angles = RingMover.transform.rotation.eulerAngles;
        RingMover.transform.rotation = Quaternion.Euler(angles.x, angles.y, angles.z + ElevatorRotationSpeed);
    }
    private void Left()
    {
        Vector3 angles = RingMover.transform.rotation.eulerAngles;
        RingMover.transform.rotation = Quaternion.Euler(angles.x, angles.y, angles.z - ElevatorRotationSpeed);
    }
    private void Forward()
    {
        float max = 0.0508f;
        if(HandMover.transform.localPosition.x < max)
        {
            HandMover.transform.position += HandMover.transform.right * Time.deltaTime * ElevatorVerticalSpeed;
        }
    }
    private void Back()
    {
        float min = -0.0377f;
        if(HandMover.transform.localPosition.x > min)
        {
            HandMover.transform.position += -HandMover.transform.right * Time.deltaTime * ElevatorVerticalSpeed;
        }
    }


    public void AttachBicycle(GameObject bicycle)
    {
        if (_bicycle == null)
        {
            _bicycle = bicycle;
            _bicycle.GetComponent<FixedJoint>().connectedBody = HandMover.GetComponent<Rigidbody>();
            _bicycle.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    private void DeattachBicycle()
    {
        _bicycle.transform.parent = BicyclesArray.transform;
    }

}
