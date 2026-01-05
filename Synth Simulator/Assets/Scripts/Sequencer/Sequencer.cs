using UnityEngine;
using UnityEngine.UI;

public class Sequencer : MonoBehaviour
{
    public InstrumentController monosynth;

    private int frame = 0;
    private Image sprite;

    private void Start()
    {
        sprite = GetComponent<Image>();
    }

    public void FixedUpdate()
    {
        frame++;

        if (frame % 60 == 0)
        {
            monosynth.NoteStart(60);
            sprite.color = Color.red;
        }

        if (frame % 60 == 30)
        {
            monosynth.NoteStop(60);
            sprite.color = Color.white;
        }
    }
}
