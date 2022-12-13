// Author: Felipe Berquó
// E-mail: felipeberquo@gmail.com
// www.felipegamedev.com

using UnityEngine;
using UnityEngine.EventSystems;

public class RoomObject : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public bool IsPlayer { get; internal set; }
    public bool IsMovable { get; internal set; }
    public GridPosition CurrentGridPosition { get; private set; }
    public Vector2 CurrentFloorPosition { get { return new Vector2(CurrentGridPosition.X * FloorSlotSize.x, -CurrentGridPosition.Y * FloorSlotSize.y); } }
    public Vector2 FloorSlotSize { get; internal set; }
    public int ColliderGridWidth { get { return (int)(GetColliderRect().width / FloorSlotSize.x); } }
    public int ColliderGridHeight { get { return (int)(Mathf.Abs(GetColliderRect().height) / FloorSlotSize.y); } }

    public virtual void Initialize(Vector2 p_floorSlotSize)
    {
        FloorSlotSize = p_floorSlotSize;
        CurrentGridPosition = new GridPosition();
        UpdateCurrentGridPositionByAnchoredPosition();
        SnapAnchoredPositionToGridPosition();
    }

    protected void UpdateCurrentGridPositionByAnchoredPosition()
    {
        RectTransform __rTrans = (RectTransform)transform;

        CurrentGridPosition.X = Mathf.RoundToInt(__rTrans.anchoredPosition.x / FloorSlotSize.x);
        CurrentGridPosition.Y = Mathf.RoundToInt(Mathf.Abs(__rTrans.anchoredPosition.y) / FloorSlotSize.y);
    }

    protected void SnapAnchoredPositionToGridPosition()
    {
        ((RectTransform)transform).anchoredPosition = CurrentFloorPosition;
    }

    public Rect GetColliderRect()
    {
        RectTransform __rT = (RectTransform)transform;

        return Rect.MinMaxRect(__rT.anchoredPosition.x, __rT.anchoredPosition.y, __rT.anchoredPosition.x + __rT.rect.size.x, __rT.anchoredPosition.y - __rT.rect.size.y);
    }

    public virtual void OnSelect(BaseEventData p_eventData)
    {

    }

    public virtual void OnDeselect(BaseEventData p_eventData)
    {

    }
}
