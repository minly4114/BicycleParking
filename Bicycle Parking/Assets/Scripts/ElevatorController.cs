using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour {

    public enum States {Up, Down, Right, Left, Forward, Back, Idle };
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
    public int level = 0;
    private int currentLevel = 0;
    public int angle = 0;
    private int currentAngle = 0;
    private float max = 0.0508f;
    private float min = -0.0377f;
    public bool isCompleted = true;
    public bool desiredState = false;
    public bool isBicycleSet = false;

    private void Start()
    {
        isCompleted = true;
        SetLevelAndAngle(1, 0);
    }

    private void FixedUpdate()
    {
        DoAction();
        Debug.Log(isCompleted);
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
            case States.Forward:
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
        if (HandMover.transform.localPosition.x < max)
        {
            HandMover.transform.position += HandMover.transform.right * Time.deltaTime * ElevatorVerticalSpeed;
        }
        else InDesiredPosition();
    }
    
    private void Back()
    {
        if (HandMover.transform.localPosition.x > min)
        {
            HandMover.transform.position += -HandMover.transform.right * Time.deltaTime * ElevatorVerticalSpeed;
        }
        else
        {
            desiredState = true;
            Planning();
        }
    }

    //присоединяем велосипед
    public void AttachBicycle(GameObject bicycle)
    {
        if (!isBicycleSet && isCompleted)
        {
            _bicycle = bicycle;
            _bicycle.GetComponent<FixedJoint>().connectedBody = HandMover.GetComponent<Rigidbody>();
            _bicycle.GetComponent<Rigidbody>().isKinematic = false;
            isBicycleSet = true;
            SetLevelAndAngle(2, 0);
        }   
    }
    //отсоединяем велосипед
    private void DeattachBicycle()
    {
        if(isBicycleSet)
        {
            _bicycle.GetComponent<FixedJoint>().connectedBody = null;
            _bicycle.GetComponent<Rigidbody>().isKinematic = true;
            _bicycle = null;
        }
    }
    //устанавливает куда нам нужно попасть
    public void SetLevelAndAngle(int level, int angle)
    {
        this.level = level;
        this.angle = angle;
        State = States.Back;
        isCompleted = false;
        desiredState = false;
        if(_bicycle!=null)
        {
            isBicycleSet = true;
        }
    }
    //меняет текущее положение
    public void TriggerEvent(string name)
    {
        if(name[0]=='L')
        {
            currentLevel = name[1] - 48;
        }
        else if(name[0]=='R')
        {
            currentAngle = name[1] - 48;
        }
        Planning();
    }
    //говорит куда ехать
    private void Planning()
    {
        if (desiredState)
        {
            if (currentLevel == level)
            {
                if (currentAngle == angle)
                {
                    PlaceBike();            
                }
                else if (currentAngle < angle)
                {
                    State = States.Right;
                    isCompleted = false;
                }
                else if (currentAngle > angle)
                {
                    State = States.Left;
                    isCompleted = false;
                }
            }
            else if (currentLevel < level)
            {
                State = States.Down;
                isCompleted = false;
            }
            else if (currentLevel > level)
            {
                State = States.Up;
                isCompleted = false;
            }
        }        
    }
    //спабатывает когда мы выдвинулись
    private void InDesiredPosition()
    {
        if(level==0&&angle==0)
        {
            State = States.Idle;
            isCompleted = true;
        }
        else
        {
            DeattachBicycle();
            level = 0;
            angle = 0;
            State = States.Back;
        }
    }
    //выдвигается на ненулевом этаже
    private void PlaceBike()
    {
        if(!isCompleted)
        {
            State = States.Forward;
        }
    }
}
