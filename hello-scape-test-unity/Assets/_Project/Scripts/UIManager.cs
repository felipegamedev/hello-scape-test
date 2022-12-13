using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; internal set; }

    private RectTransform _moveCountContainerRTrans;
    private TMP_Text _moveCountText;
    private RectTransform _centerMessageContainerRTrans;
    private TMP_Text _centerMessageText;
    private Image _levelCompletedScreenBackground;
    private TMP_Text _levelCompletedScreenTitle;
    private GameObject _levelCompleteScreenStarsContainerGObj;
    private RectTransform _levelCompleteScreenMoveCountContainerRTrans;
    private TMP_Text _levelCompleteScreenMoveCountText;
    private Button _levelCompleteScreenRestartButton;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);

            return;
        }

        Instance = this;
        _moveCountContainerRTrans = transform.Find("MoveCountContainer").GetComponent<RectTransform>();
        _moveCountText = _moveCountContainerRTrans.Find("MoveCountText").GetComponent<TMP_Text>();
        _moveCountContainerRTrans.gameObject.SetActive(false);
        ResetMoveCountContainer();
        _centerMessageContainerRTrans = transform.Find("CenterMessageContainer").GetComponent<RectTransform>();
        _centerMessageText = _centerMessageContainerRTrans.Find("MessageText").GetComponent<TMP_Text>();
        _centerMessageContainerRTrans.gameObject.SetActive(false);
        ResetCenterMessageContainer();
        _levelCompletedScreenBackground = transform.Find("LevelCompletedScreen").GetComponent<Image>();
        _levelCompletedScreenTitle = _levelCompletedScreenBackground.transform.Find("LevelCompletedLabel").GetComponent<TMP_Text>();
        _levelCompleteScreenStarsContainerGObj = _levelCompletedScreenBackground.transform.Find("StarsContainer").gameObject;
        _levelCompleteScreenMoveCountContainerRTrans = _levelCompletedScreenBackground.transform.Find("MoveCountContainer/MoveCountText").GetComponent<RectTransform>();
        _levelCompleteScreenMoveCountText = _levelCompletedScreenBackground.transform.Find("MoveCountContainer/MoveCountText").GetComponent<TMP_Text>();
        _levelCompleteScreenRestartButton = _levelCompletedScreenBackground.transform.Find("RestarButton").GetComponent<Button>();
        ResetLevelCompletedScreen();
    }

    public void ShowTextInCenterMessageContainer(string p_text, Action p_onComplete, bool p_animated = true)
    {
        Image __containerBackground = _centerMessageContainerRTrans.GetComponent<Image>();

        ResetCenterMessageContainer();
        _centerMessageContainerRTrans.gameObject.SetActive(true);

        if (p_animated)
        {
            __containerBackground.DOFade(0.5f, 0.35f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                _centerMessageText.DOText(p_text, 0.5f).OnComplete(() =>
                {
                    p_onComplete?.Invoke();
                });
            });
        }
        else
        {
            __containerBackground.DOFade(0.5f, 0f);
            _centerMessageText.text = p_text;
            p_onComplete?.Invoke();
        }
    }

    public void HideCenterMessageContainer(Action p_onComplete, bool p_animated = true)
    {
        Image __containerBackground = _centerMessageContainerRTrans.GetComponent<Image>();

        if (p_animated)
        {
            _centerMessageText.DOText(string.Empty, 0.15f).OnComplete(() =>
            {
                __containerBackground.DOColor(Color.clear, 0.15f).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    _centerMessageContainerRTrans.gameObject.SetActive(false);
                    p_onComplete?.Invoke();
                });
            });
        }
        else
        {
            __containerBackground.color = Color.clear;
            _centerMessageContainerRTrans.gameObject.SetActive(false);
            p_onComplete?.Invoke();
        }
    }

    private void ResetCenterMessageContainer()
    {
        _centerMessageContainerRTrans.GetComponent<Image>().color = Color.clear;
        _centerMessageText.text = string.Empty;
    }

    public void ShowMoveCountContainer(Action p_onComplete, bool p_animated = true)
    {
        ResetMoveCountContainer();
        _moveCountContainerRTrans.gameObject.SetActive(true);

        if (p_animated)
        {
            _moveCountContainerRTrans.DOAnchorPosY(8f, 0.35f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                p_onComplete?.Invoke();
            });
        }
        else
        {
            _moveCountContainerRTrans.anchoredPosition = 8f * Vector2.up;
            p_onComplete?.Invoke();
        }
    }

    private void ResetMoveCountContainer()
    {
        _moveCountContainerRTrans.anchoredPosition = _moveCountContainerRTrans.sizeDelta.y * Vector2.down;
    }

    public void ShowLevelCompletedScreen(int p_moveCount, Action p_onComplete, bool p_animated = true)
    {
        ResetLevelCompletedScreen();
        _levelCompletedScreenBackground.gameObject.SetActive(true);

        if (p_animated)
        {
            _levelCompletedScreenBackground.DOFade(1f, 0.35f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                _levelCompleteScreenStarsContainerGObj.SetActive(true);
                _levelCompleteScreenMoveCountContainerRTrans.gameObject.SetActive(true);
                DOVirtual.Int(0, p_moveCount, 0.25f, p_count =>
                {
                    _levelCompleteScreenMoveCountText.text = p_count.ToString();
                }).OnComplete(() =>
                {
                    _levelCompleteScreenRestartButton.gameObject.SetActive(true);
                    p_onComplete?.Invoke();
                });
            });
        }
        else
        {
            p_onComplete?.Invoke();
        }
    }

    public void HideLevelCompletedScreen(Action p_onComplete, bool p_animated = true)
    {
        Image __containerBackground = _centerMessageContainerRTrans.GetComponent<Image>();

        _levelCompleteScreenStarsContainerGObj.SetActive(false);
        _levelCompletedScreenTitle.gameObject.SetActive(false);
        _levelCompleteScreenMoveCountContainerRTrans.gameObject.SetActive(false);
        _levelCompleteScreenRestartButton.gameObject.SetActive(false);

        if (p_animated)
        {
            _levelCompletedScreenBackground.DOFade(0f, 0.15f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                _levelCompletedScreenBackground.gameObject.SetActive(false);
                p_onComplete?.Invoke();
            });
        }
        else
        {
            Color __c = _levelCompletedScreenBackground.color;

            __c.a = 0f;
            _levelCompletedScreenBackground.color = __c;
            _levelCompletedScreenBackground.gameObject.SetActive(false);
            p_onComplete?.Invoke();
        }
    }

    private void ResetLevelCompletedScreen()
    {
        _levelCompleteScreenStarsContainerGObj.SetActive(false);
        _levelCompletedScreenBackground.DOFade(0f, 0f);
        _levelCompleteScreenMoveCountText.text = "0";
    }

    public void UpdateMoveCountText(string p_countText)
    {
        _moveCountText.text = p_countText;
    }
}
