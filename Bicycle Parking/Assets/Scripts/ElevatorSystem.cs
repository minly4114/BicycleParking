using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElevatorSystem : MonoBehaviour
{
    public UIController uiController;
	Dictionary<int?, string> ParkingMap;
    ElevatorController controller;
    private const int ParkingCount = 32;
    private string lastCardId;
    private bool AttachAlertMessage = false;
    private bool ChooseActionAlert = false;

    private void Start()
    {
        InitializeParking();
        controller = GameObject.Find("BlackBox").GetComponent<ElevatorController>();
    }


    private void Update()
    {
        if (!controller.IsBicycleAttached() && !controller.isCompleted)
        {
            uiController.PromptStatus("Please wait...");
        }
        else if (controller.isCompleted)
        {
            uiController.PromptStatus("Attach the card");
        }
        if (AttachAlertMessage)
        {
            uiController.PromptStatus("Attach the bicycle!");
        }
        if (ChooseActionAlert)
        {
            uiController.PromptStatus("Choose action");
        }
    }

    public void FindNewCard (string cardId)
	{
        var parkingPlaces = ParkingMap.Where(a => a.Value == cardId).Select(a => a.Key);
        if (controller.isCompleted)
        {
            if (parkingPlaces.Count() > 0)
            {
                ChooseActionAlert = true;
                List<string> result = new List<string>();
                foreach(var i in parkingPlaces)
                {
                    result.Add(i.Value.ToString());
                }
                uiController.PromptBicycles(result);
                lastCardId = cardId;
                AttachAlertMessage = false;
            }
            else
            {
                if (controller.IsBicycleAttached())
                {
                    uiController.PromptStatus("Parking...");
                    var parkingPlace = ParkingMap.FirstOrDefault(a => a.Value == null);
                    ParkingMap[parkingPlace.Key] = cardId;
                    controller.BicycleParkingEvent(
                        GetParkingLevel((int)parkingPlace.Key),
                        GetParkingAngle((int)parkingPlace.Key),
                        true);
                    AttachAlertMessage = false;
                    ChooseActionAlert = false;
                } else
                {
                    AttachAlertMessage = true;
                    ChooseActionAlert = false;
                }
            }
        }
    }

    private int GetParkingLevel(int key)
    {
        if(key < 8)
        {
            return 1;
        } else if(key < 16)
        {
            return 2;
        } else if(key < 24)
        {
            return 3;
        } else if(key < 32)
        {
            return 4;
        }
        return 0;
    }

    private int GetParkingAngle(int key)
    {
        if (key < 8)
        {
            return key;
        }
        else if (key < 16)
        {
            return key - 8;
        }
        else if (key < 24)
        {
            return key - 16;
        }
        else if (key < 32)
        {
            return key - 24;
        }
        return 0;
    }

	private void InitializeParking ()
	{
		ParkingMap = new Dictionary<int?, string> ();
		for (int i = 0; i < ParkingCount; i++)
		{
			ParkingMap.Add (i, null);
		}
	}

    public void SelectedParking(int parkingNumber)
    {
        if (controller.IsBicycleAttached())
        {
            uiController.PromptStatus("Deattach the bicycle!");
            AttachAlertMessage = false;
            ChooseActionAlert = false;
        }
        else
        {
            controller.BicycleParkingEvent(
                GetParkingLevel(parkingNumber),
                GetParkingAngle(parkingNumber),
                false);
            ParkingMap[parkingNumber] = null;
            AttachAlertMessage = false;
            ChooseActionAlert = false;
        }
    }

    public void ParkingBicycle()
    {
        if (!controller.IsBicycleAttached())
        {
            AttachAlertMessage = true;
            ChooseActionAlert = false;
        }
        else
        {
            var parkingPlace = ParkingMap.FirstOrDefault(a => a.Value == null);
            ParkingMap[parkingPlace.Key] = lastCardId;
            controller.BicycleParkingEvent(
                GetParkingLevel((int)parkingPlace.Key),
                GetParkingAngle((int)parkingPlace.Key),
                true);
            AttachAlertMessage = false;
            ChooseActionAlert = false;
        }
    }
}