using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;

public class SequencerCell : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private SequencerNote prefab;

    private int row_note;
    public int positionInPattern;
    private Sequencer sequencer;

    private void Start()
    {
        row_note = GetComponentInParent<SequencerRow>().note;
        sequencer = GetComponentInParent<Sequencer>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SequencerNote new_note = Instantiate(prefab, this.transform);
        new_note.note = row_note;
        new_note.startPositionInPattern = positionInPattern;
        sequencer.RebuildSchedule();
    }
}
