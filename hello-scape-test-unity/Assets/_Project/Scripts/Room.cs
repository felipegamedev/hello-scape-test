// Author: Felipe Berquó
// E-mail: felipeberquo@gmail.com
// www.felipegamedev.com

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public Vector2 FloorSize { get; private set; }
    public Vector2 FloorSlotSize { get; private set; }
    public int FloorColumnsCount { get { return (int)(FloorSize.x / FloorSlotSize.x); } }
    public int FloorRowsCount { get { return (int)(FloorSize.y / FloorSlotSize.y); } }

    public Action OnObjectMoved;
    public Action OnPlayerReachedExit;

    private GridLayoutGroup _floorGrid;
    private RectTransform _roomObjectsParent;
    private List<RoomObject> _roomObjects;
    private Player _player;

    public void Initialize()
    {
        Canvas.ForceUpdateCanvases();
        _floorGrid = transform.Find("FloorGrid").GetComponent<GridLayoutGroup>();
        FloorSize = _floorGrid.GetComponent<RectTransform>().rect.size;
        FloorSlotSize = new Vector2(_floorGrid.cellSize.x, _floorGrid.cellSize.y);
        _roomObjectsParent = transform.Find("Objects").GetComponent<RectTransform>();
        _roomObjectsParent.sizeDelta = FloorSize;
        InitializeRoomObjects();
    }

    private void InitializeRoomObjects()
    {
        _roomObjects = new List<RoomObject>();

        for (int i = 0; i < _roomObjectsParent.childCount; i++)
        {
            RoomObject __roomObject = _roomObjectsParent.GetChild(i).GetComponent<RoomObject>();

            __roomObject.Initialize(FloorSlotSize);

            if (__roomObject.IsMovable)
            {
                MovableRoomObject __movableRoomObject = ((MovableRoomObject)__roomObject);

                __movableRoomObject.OnSelected = OnMovableObjectSelectedHandler;
                __movableRoomObject.OnMoved = OnMovableObjectMovedHandler;

                if (__roomObject.IsPlayer)
                    _player = (Player)__movableRoomObject;
            }

            _roomObjects.Add(__roomObject);
        }
    }

    private void OnMovableObjectSelectedHandler(MovableRoomObject p_selectedObject)
    {
        p_selectedObject.UpdateMoveArrows(GetRoomObjectAllowedMoveDirections(p_selectedObject));
        p_selectedObject.transform.SetAsLastSibling();
    }

    private void OnMovableObjectMovedHandler(MovableRoomObject p_movedObject)
    {
        if (p_movedObject.IsPlayer && p_movedObject.CurrentGridPosition.X == 5 && p_movedObject.CurrentGridPosition.Y == 4)
        {
            OnPlayerReachedExit?.Invoke();
            Debug.LogWarning("PLAYER REACHED EXIT");
        }
        else
        {
            OnMovableObjectSelectedHandler(p_movedObject);
        }

        OnObjectMoved?.Invoke();
    }

    private bool[] GetRoomObjectAllowedMoveDirections(MovableRoomObject p_object)
    {
        GridPosition __gridPos = p_object.CurrentGridPosition;
        int __gridWidth = p_object.ColliderGridWidth;
        int __gridHeight = p_object.ColliderGridHeight;
        bool[] __collisionWithOtherObjectsDirections = GetCollisionWithOtherObjectsDirections(p_object);
        bool[] __allowedMoveDirections = new bool[4];

        // Left: check if is out of bounds or if there's an object.
        if (__gridPos.X > 0 && !__collisionWithOtherObjectsDirections[0])
            __allowedMoveDirections[0] = true;

        // Up: check if is out of bounds or if there's an object.
        if (__gridPos.Y > 0 && !__collisionWithOtherObjectsDirections[1])
            __allowedMoveDirections[1] = true;

        // Right: check if is out of bounds or if there's an object.
        if (__gridPos.X + __gridWidth < FloorColumnsCount && !__collisionWithOtherObjectsDirections[2])
            __allowedMoveDirections[2] = true;

        // Down: check if is out of bounds or if there's an object.
        if (__gridPos.Y + __gridHeight < FloorRowsCount && !__collisionWithOtherObjectsDirections[3])
            __allowedMoveDirections[3] = true;

        return __allowedMoveDirections;
    }

    private bool[] GetCollisionWithOtherObjectsDirections(MovableRoomObject p_object)
    {
        Rect __colliderRect = p_object.GetColliderRect();
        Rect __leftColliderRect = new Rect(__colliderRect.position + FloorSlotSize.x * Vector2.left, __colliderRect.size);
        Rect __upColliderRect = new Rect(__colliderRect.position + FloorSlotSize.y * Vector2.up, __colliderRect.size);
        Rect __rightColliderRect = new Rect(__colliderRect.position + FloorSlotSize.x * Vector2.right, __colliderRect.size);
        Rect __downColliderRect = new Rect(__colliderRect.position + FloorSlotSize.y * Vector2.down, __colliderRect.size);
        bool[] __collisionDetectedDirections = new bool[4];

        foreach (RoomObject roomObject in _roomObjects)
        {
            // Don't check with the object itself.
            if (roomObject == p_object)
                continue;

            // Left collisions check.
            if (!__collisionDetectedDirections[0])
                __collisionDetectedDirections[0] = __leftColliderRect.Overlaps(roomObject.GetColliderRect(), true);

            // Up collisions check.
            if (!__collisionDetectedDirections[1])
                __collisionDetectedDirections[1] = __upColliderRect.Overlaps(roomObject.GetColliderRect(), true);

            // Right collisions check.
            if (!__collisionDetectedDirections[2])
                __collisionDetectedDirections[2] = __rightColliderRect.Overlaps(roomObject.GetColliderRect(), true);

            // Down collisions check.
            if (!__collisionDetectedDirections[3])
                __collisionDetectedDirections[3] = __downColliderRect.Overlaps(roomObject.GetColliderRect(), true);
        }

        return __collisionDetectedDirections;
    }
}
