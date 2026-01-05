using UnityEngine;
using System.Linq;

/// <summary>
/// Receives notes from the sequencer and plays them on the synth, as if it was a person pressing the keys.
/// Its job is not to interpret keypresses. It just recieves note-start and note-stop messages from the sequencer,
/// and then pushes and releases the keys on the synth using the same methods as if they were being clicked
/// by a mouse.
/// </summary>
public class MonosynthController : InstrumentController
{

    private SynthKey[] keys;
    
    private void Start()
    {
        keys = GetComponentsInChildren<SynthKey>();
    }

    public override void NoteStart(int val)
    {
        // find the key whose note matches val and do keydown
        SynthKey key = keys.FirstOrDefault(k => k.note == val);
        key.KeyDown();
    }

    public override void NoteStop(int val) 
    {
        SynthKey key = keys.FirstOrDefault(k => k.note == val);
        key.KeyUp();
    }

}
