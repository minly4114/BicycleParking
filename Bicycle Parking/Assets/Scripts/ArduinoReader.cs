using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

public class ArduinoReader
{
	private ElevatorSystem _elSys;
	private SerialPort sPort;

	public ArduinoReader (ElevatorSystem elSys)
	{
		_elSys = elSys;
        sPort = new SerialPort("COM7");	
        Task task = new Task(() => ReadCycle());
        task.Start();
	}

	private void ReadCycle ()
	{
		try
		{
            sPort.Open();
			while (true)
			{
				if (sPort.BytesToRead > 0)
				{
					string msg = sPort.ReadLine ();
					_elSys.FindNewCard (msg);
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