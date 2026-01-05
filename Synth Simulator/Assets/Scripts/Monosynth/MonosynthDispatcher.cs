using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonosynthDispatcher : MonoBehaviour
{
	
	public LibPdInstance pdPatch;

	private int current_note;
	private List<SynthKey> held_notes;
	private List<SynthKey> keys;
	private SynthKey last_human_pushed;

	private void Start()
	{
		foreach (UiControlBase item in GetComponentsInChildren< UiControlBase>())
		{
			item.dispatcher = this;
        }
        keys = new List<SynthKey>(GetComponentsInChildren<SynthKey>());
		held_notes = new List<SynthKey>();
    }

    private void Update()
    {
		if (Input.GetMouseButtonUp(0)) { KeyReleased(last_human_pushed); }
    }

    public void KeyPressed(SynthKey key, bool human_pushed = false)
	{
		held_notes.Add(key);
		current_note = key.note;
		UpdateNote();
        SendOn();
		UpdateKeyShading();
		if (human_pushed) { last_human_pushed = key; }
    }

	public void KeyReleased(SynthKey key)
	{
		held_notes.Remove(key);
		if (held_notes.Count == 0)
		{
			SendOff();
		}
		else
		{
			current_note = held_notes[held_notes.Count - 1].note;
            UpdateNote();
        }
		UpdateKeyShading();
    }

	private void UpdateKeyShading()
	{
		foreach (SynthKey key in keys)
		{
			key.Release();
		}
		if (held_notes.Count > 0)
		{
			held_notes[held_notes.Count - 1].Tint();
        }
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

