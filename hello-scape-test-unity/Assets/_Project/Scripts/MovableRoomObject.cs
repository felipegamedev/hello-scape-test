// Author: Felipe Berquó
// E-mail: felipeberquo@gmail.com
// www.felipegamedev.com

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MovableRoomObject : RoomObject
{
    public Action<MovableRoomObject> OnSelected;
    public Action<MovableRoomObject> OnMoved;

    protected Transform moveArrowsParent;
    protected Transform selectedTr;

    public override void Initialize(Vector2 p_floorSlotSize)
    {
        base.Initialize(p_floorSlotSize);
        IsMovable = true;
        moveArrowsParent = transform.Find("MoveArrows");
        moveArrowsParent.gameObject.SetActive(false);
        selectedTr = transform.Find("Selected");
        selectedTr.transform.localScale = Vector3.zero;
        selectedTr.gameObject.SetActive(false);
    }

    public override void OnSelect(BaseEventData p_eventData)
    {
        base.OnSelect(p_eventData);
        moveArrowsParent.gameObject.SetActive(true);
        selectedTr.gameObject.SetActive(true);
        selectedTr.transform.DOScale(1f, 0.1f).SetEase(Ease.OutCubic);
        OnSelected?.Invoke(this);
    }

    public override void OnDeselect(BaseEventData p_eventData)
    {
        base.OnDeselect(p_eventData);
        moveArrowsParent.gameObject.SetActive(false);
        selectedTr.gameObject.SetActive(false);
        selectedTr.transform.localScale = Vector3.zero;
    }

    public void UpdateMoveArrows(bool[] p_allowedMoveDirections)
    {
        moveArrowsParent.Find("Left").gameObject.SetActive(p_allowedMoveDirections[0]);
        moveArrowsParent.Find("Up").gameObject.SetActive(p_allowedMoveDirections[1]);
        moveArrowsParent.Find("Right").gameObject.SetActive(p_allowedMoveDirections[2]);
        moveArrowsParent.Find("Down").gameObject.SetActive(p_allowedMoveDirections[3]);
    }

    public void MoveLeft()
    {
        ((RectTransform)transform).anchoredPosition += FloorSlotSize.x * Vector2.left;
        OnMovedHandler();
    }

    public void MoveUp()
    {
        ((RectTransform)transform).anchoredPosition += FloorSlotSize.x * Vector2.up;
        OnMovedHandler();
    }

    public void MoveRight()
    {
        ((RectTransform)transform).anchoredPosition += FloorSlotSize.x * Vector2.right;
        OnMovedHandler();
    }

    public void MoveDown()
    {
        ((RectTransform)transform).anchoredPosition += FloorSlotSize.y * Vector2.down;
        OnMovedHandler();
    }

    protected virtual void OnMovedHandler()
    {
        UpdateCurrentGridPositionByAnchoredPosition();
        SnapAnchoredPositionToGridPosition();
        OnMoved?.Invoke(this);
    }
}
