using System.Collections.Generic;
using System.Linq;

public class ElevatorSystem
{

	private ArduinoReader reader;
	Dictionary<int, string> ParkingMap;
	public ElevatorSystem ()
	{
		InitializeParking ();
		reader = new ArduinoReader (this);
	}

	public void FindNewCard (string cardId)
	{
		var parkingNumber = ParkingMap.Select (a => a.Value == cardId);
		if (parkingNumber != null)
		{
			// TODO: GetBicycle from the parking
		}
		else
		{
			var parkingPlace = ParkingMap.Select (a => a.Value == null);
			// TODO: SetBicycle to the parking
		}
	}

	private void InitializeParking ()
	{
		ParkingMap = new Dictionary<string, int> ();
		for (int i = 0; i < 100; i++)
		{
			ParkingMap.Add (i, null);
		}
	}
}