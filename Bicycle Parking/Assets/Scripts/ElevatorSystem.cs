using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElevatorSystem : MonoBehaviour
{
	Dictionary<int?, string> ParkingMap;

    private void Start()
    {
        InitializeParking();
        new ArduinoReader(this);
    }

    public void FindNewCard (string cardId)
	{
        var parkingPlaces = ParkingMap.Where(a => a.Value == cardId).Select(a => a.Key);

        if (parkingPlaces.Count() > 0)
        {

            // TODO: GetBicycle from the parking
        }
        else
        {
            var parkingPlace = ParkingMap.FirstOrDefault(a => a.Value == null);
            ParkingMap[parkingPlace.Key] = cardId;

            // TODO: SetBicycle to the parking
        }
    }

	private void InitializeParking ()
	{
		ParkingMap = new Dictionary<int?, string> ();
		for (int i = 0; i < 100; i++)
		{
			ParkingMap.Add (i, null);
		}
	}

    public void WriteLog(string msg)
    {
        Debug.Log(msg);
    }
}