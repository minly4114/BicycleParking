using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour {

    public enum States {Up, Down, Right, Left, Forward, Back, Idle };
    public States State = States.Idle;

    public float ElevatorVerticalSpeed = 2.0f;
    public float ElevatorRotationSpeed = 0.7f;

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
    private readonly float max = 0.051f;
    private readonly float min = -0.0377f;
    public bool isCompleted = true;
    public bool desiredState = false;
    public bool isBicycleSet = false;
    public bool isTaskCompleted = false;

    private void Start()
    {
        isCompleted = true;
    }

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
            case States.Forward:
                Forward();
                break;
            case States.Back:
                Back();
                break;
        }
    }

    #region Movement

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

    #endregion

    #region BicycleActions

    //присоединяем велосипед
    public void AttachBicycle(GameObject bicycle)
    {
        if (!isTaskCompleted)
        {
            _bicycle = bicycle;
            Attach();
        }
    }

    private void Attach()
    {     
        _bicycle.GetComponent<FixedJoint>().connectedBody = HandMover.GetComponent<Rigidbody>();
        _bicycle.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void BicycleActions()
    {
        if (!isTaskCompleted)
        {
            if (isBicycleSet)
            {
                _bicycle.GetComponent<FixedJoint>().connectedBody = null;
                _bicycle.GetComponent<Rigidbody>().isKinematic = true;
                _bicycle = null;
                isBicycleSet = false;
                isTaskCompleted = true;
            }
            else if (!isBicycleSet)
            {
                isTaskCompleted = true;
                Attach();
            }
        }
    }

    #endregion


    public bool BicycleParkingEvent(int level, int angle, bool setBike)
    {
        this.level = level;
        this.angle = angle;      
        isCompleted = false;
        desiredState = false;
        isBicycleSet = setBike;
        State = States.Back;
        return true;
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
            if (_bicycle != null)
            {
                _bicycle.GetComponent<FixedJoint>().connectedBody = null;
                _bicycle.GetComponent<Rigidbody>().isKinematic = true;
                _bicycle.transform.position += _bicycle.transform.right * 0.2f;
                _bicycle = null;
            }
            isTaskCompleted = false;       
        }
        else
        {
            BicycleActions();
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

    public bool IsBicycleAttached()
    {
        return _bicycle != null;
    }
}
