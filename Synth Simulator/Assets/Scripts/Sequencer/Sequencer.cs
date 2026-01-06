using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Sequencer : MonoBehaviour
{
    public InstrumentController monosynth;

    // children
    private Image pointer;
    private Vector3 pointerStartPos;
    private RectTransform rectTransform;

    // music controls
    public int bpm = 120;
    private int patternLength = 16;

    // processing and maths
    private float timePerBeat;
    private float timePerPattern;
    private float timeSinceStartOfPattern = 0.0f;
    private float positionInPattern = 0.0f;
    private float lastPosition = 0.0f;

    // instruction scheduling
    private List<SequencerInstruction> schedule;

    private void Start()
    {
        // maths
        timePerBeat = 60f / (bpm * 4f);
        timePerPattern = timePerBeat * patternLength;

        // initialisation
        pointer = GetComponentInChildren<Image>();
        pointerStartPos = pointer.rectTransform.localPosition;
        rectTransform = GetComponent<RectTransform>();
        schedule = new List<SequencerInstruction>();
    }

    private void Update()
    {
        // passage of time
        timeSinceStartOfPattern += Time.deltaTime;

        // store last position and calculate new position
        lastPosition = positionInPattern;
        positionInPattern = timeSinceStartOfPattern / timePerPattern;

        // resolve everything between last check and current check
        ResolveRange(lastPosition, positionInPattern);

        // wrap if at end
        if (positionInPattern >= 1f)
        {
            timeSinceStartOfPattern -= timePerPattern;
            positionInPattern = timeSinceStartOfPattern / timePerPattern;

            // catch any events at 0
            ResolveRange(0, positionInPattern);
        }
        
        // move pointer head
        pointer.rectTransform.localPosition = pointerStartPos + new Vector3(rectTransform.rect.width * positionInPattern, 0f, 0f);
    }

    private void ResolveRange(float from, float to)
    {
        foreach (SequencerInstruction item in schedule)
        {
            if (from <= item.schedulePosition && item.schedulePosition < to)
            {
                ResolveInstruction(item);
            }
        }
    }

    public void ResolveInstruction(SequencerInstruction i)
    {
        if (i.instruction == SequencerInstruction.InstructionType.NOTE_ON)
        {
            monosynth.NoteStart(i.instructionValue);
        }
        else if (i.instruction == SequencerInstruction.InstructionType.NOTE_OFF)
        {
            monosynth.NoteStop(i.instructionValue);
        }
    }

    public void RebuildSchedule(bool reset = false)
    {
        List<SequencerInstruction> temp_schedule = new List<SequencerInstruction>();

        foreach (SequencerNote note in GetComponentsInChildren<SequencerNote>())
        {
            SequencerInstruction on = new SequencerInstruction();
            on.instruction = SequencerInstruction.InstructionType.NOTE_ON;
            on.instructionValue = note.note;
            on.schedulePosition = (float)note.startPositionInPattern / patternLength;
            temp_schedule.Add(on);

            SequencerInstruction off = new SequencerInstruction();
            off.instruction = SequencerInstruction.InstructionType.NOTE_OFF;
            off.instructionValue = note.note;
            off.schedulePosition = (float)(note.startPositionInPattern + 0.99f) / patternLength;
            temp_schedule.Add(off);
        }
        schedule = temp_schedule.OrderBy(p => p.schedulePosition).ThenBy(p => p.instructionValue).ToList();
    }
}

public class SequencerInstruction
{
    public enum InstructionType { NOTE_ON, NOTE_OFF }
    public InstructionType instruction;
    public int instructionValue;
    public float schedulePosition;

}