using UnityEngine;

public class PdDispatcher : MonoBehaviour
{
	
	public LibPdInstance pdPatch;

	private int current_note;

	public void KeyDown()
	{
		SendOn();
	}

	public void RecieveNote(int _note)
	{
		current_note = _note;
		UpdateNote();
	}

	public void KeyUp()
	{
		SendOff();
	}

	public void SendOn()
	{
		pdPatch.SendBang("triggeron");
	}

	public void UpdateNote()
	{
		pdPatch.SendFloat("midi_note", current_note);
	}

	public void UpdateParam(string param_name, float param_value)
	{
		pdPatch.SendFloat(param_name, param_value);
	}

	public void SendOff()
	{
		pdPatch.SendBang("triggeroff");		
	}
}

