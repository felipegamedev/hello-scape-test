// Author: Felipe Berquó
// E-mail: felipeberquo@gmail.com
// www.felipegamedev.com

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Room CurrentRoom;

    private GraphicRaycaster _gameGraphicRaycaster;
    private int _moveCount;

    private void Awake()
    {
        _gameGraphicRaycaster = GameObject.Find("Game").GetComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        ToggleInputs(false);
        CurrentRoom.Initialize();
        CurrentRoom.OnObjectMoved = OnRoomObjectMovedHandler;
        CurrentRoom.OnPlayerReachedExit = OnPlayerReachedExitHandler;
        UIManager.Instance.ShowTextInCenterMessageContainer("REACH THE EXIT", () =>
        {
            WaitForSeconds(2f, () =>
            {
                UIManager.Instance.ShowTextInCenterMessageContainer("PLAY!", () =>
                {
                    WaitForSeconds(1f, () =>
                    {
                        UIManager.Instance.HideCenterMessageContainer(() =>
                        {
                            ToggleInputs(true);
                            UIManager.Instance.ShowMoveCountContainer(null);
                        });
                    });
                }, false);
            });
        });
    }

    private void OnRoomObjectMovedHandler()
    {
        _moveCount++;
        UIManager.Instance.UpdateMoveCountText(_moveCount.ToString());
        Debug.Log("Move count: " + _moveCount);
    }

    private void OnPlayerReachedExitHandler()
    {
        ToggleInputs(false);
        WaitForSeconds(0.5f, () =>
        {
            UIManager.Instance.ShowLevelCompletedScreen(_moveCount, null);
        });
    }

    private void ToggleInputs(bool p_enabled)
    {
        _gameGraphicRaycaster.enabled = p_enabled;
    }

    private Coroutine WaitForSeconds(float p_seconds, Action p_onComplete)
    {
        return StartCoroutine(WaitForSecondsCoroutine(p_seconds, p_onComplete));
    }

    private IEnumerator WaitForSecondsCoroutine(float p_seconds, Action p_onComplete)
    {
        yield return new WaitForSeconds(p_seconds);

        p_onComplete?.Invoke();
    }

    public void OnResetGameButtonClickedHandler()
    {
        SceneManager.LoadScene("Game");
    }

    private void ResetGame()
    {
        _moveCount = 0;
    }
}
