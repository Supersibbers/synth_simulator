using UnityEngine;

public class SequencerRow : MonoBehaviour
{
    public int note;

    private void Start()
    {
        int i = 0;
        foreach (SequencerCell child in GetComponentsInChildren<SequencerCell>())
        {
            child.positionInPattern = i;
            i++;
        }
    }
}
