using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    private float headerPosition = 0.0f;
    private float positionInPattern = 0.0f;

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
        positionInPattern = timeSinceStartOfPattern / timePerPattern;
        if (positionInPattern >= 1f)
        {
            timeSinceStartOfPattern -= timePerPattern;
            positionInPattern = timeSinceStartOfPattern / timePerPattern;
            RebuildSchedule(true);
        }
        pointer.rectTransform.localPosition = pointerStartPos + new Vector3(rectTransform.rect.width*positionInPattern, 0f, 0f);

        if (schedule.Count > 0)
        {
            if (positionInPattern >= schedule[0].schedulePosition) 
            {
                print($"resolving {schedule[0].instruction} at {schedule[0].schedulePosition}");
                ResolveInstruction(schedule[0]);
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
        schedule.Remove(i);
    }

    public void RebuildSchedule(bool reset = false)
    {
        List<SequencerInstruction> temp_schedule = new List<SequencerInstruction>();

        foreach (SequencerNote note in GetComponentsInChildren<SequencerNote>())
        {
            SequencerInstruction on = new SequencerInstruction();
            on.instruction = SequencerInstruction.InstructionType.NOTE_ON;
            on.instructionValue = note.note;
            on.schedulePosition = (note.startPositionInPattern / 16f);
            temp_schedule.Add(on);

            SequencerInstruction off = new SequencerInstruction();
            off.instruction = SequencerInstruction.InstructionType.NOTE_OFF;
            off.instructionValue = note.note;
            off.schedulePosition = ((note.startPositionInPattern + 0.99f) / 16f);
            temp_schedule.Add(off);
        }
        
        // only keep ones which have yet to come
        foreach (SequencerInstruction i in temp_schedule)
        {
            if (i.schedulePosition >= positionInPattern || reset)
            {
                schedule.Add(i);
            }
        }
        schedule = schedule.OrderBy(p => p.schedulePosition).ThenBy(p => p.instructionValue).ToList();
    }
}

public class SequencerInstruction
{
    public enum InstructionType { NOTE_ON, NOTE_OFF }
    public InstructionType instruction;
    public int instructionValue;
    public float schedulePosition;

}