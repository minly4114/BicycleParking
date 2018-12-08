using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

public class ArduinoReader
{

	private ElevatorController _controller;
	private SerialPort sPort;

	public ArduinoReader (ElevatorController controller)
	{
		_controller = controller;
		sPort = "COM7"; // TODO: hardcoded string! We are need COM checker to find Arduino
		Task task = new Task (() => ReadCycle ()).Start ();
	}

	private void ReadCycle ()
	{
		try
		{
			sPort.Open ();
			while (true)
			{
				if (sPort.BytesToRead > 0)
				{
					string msg = sPort.ReadLine ();
					_controller.FindNewCard (msg);
				}
				Thread.Sleep (10);
			}
		}
		catch
		{
			throw;
		}
		finally
		{
			sPort.Close ();
		}

	}
}