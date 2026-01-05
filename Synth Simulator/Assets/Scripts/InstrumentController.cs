using UnityEngine;

public abstract class InstrumentController : MonoBehaviour, IPlayable
{
    public abstract void NoteStart(int val);
    public abstract void NoteStop(int val);
}

public interface IPlayable
{
    public void NoteStart(int val);
    public void NoteStop(int val);
}