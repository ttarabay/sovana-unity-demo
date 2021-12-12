using DG.Tweening;
using UnityEngine;

public class AnimPopup : MonoBehaviour
{
    [SerializeField] RectTransform frame;

    private void OnEnable()
    {
        frame.localScale = Vector2.zero;
        frame.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }
}
