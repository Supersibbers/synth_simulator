using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWeight : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public RectTransform rectTransform;
    private Vector2 initialPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.GetComponentInParent<SynthKey>())
        {
            transform.GetComponentInParent<SynthKey>().KeyUp();
        }
        parentAfterDrag = GetComponentInParent<Canvas>().transform;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        if (parentAfterDrag.GetComponent<SynthKey>())
        {
            rectTransform.anchoredPosition = new Vector2(0f, -30f);
        }
        else
        {
            rectTransform.anchoredPosition = initialPosition;
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += new Vector2(0f, 24f);
    }
}
