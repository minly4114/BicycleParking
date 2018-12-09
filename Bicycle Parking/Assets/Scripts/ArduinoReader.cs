using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ArduinoReader : MonoBehaviour
{
	public ElevatorSystem _elSys;
	private SerialPort sPort;

	void Start()
	{
        sPort = new SerialPort("COM7");
        sPort.Open();          
    }

    public void FixedUpdate()
    {
        if (sPort.BytesToRead > 0)
        {
            string msg = sPort.ReadLine();
            _elSys.FindNewCard(msg);
        }
    }
}