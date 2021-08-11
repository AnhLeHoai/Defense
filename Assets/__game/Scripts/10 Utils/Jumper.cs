using DG.Tweening;
using UnityEngine;

public class Jumper: MonoBehaviour
{
    [SerializeField] private GameObject shadow;
    [SerializeField] private float jumpHeight = 2;
    
    public float time = 1f;
    private Sequence sequence;
    private Sequence sequence2;
    

    private void Awake()
    {
        sequence = DOTween.Sequence()
            .Append(transform.DOLocalMoveY(jumpHeight, time).SetEase(Ease.OutCubic))
            .Append(transform.DOLocalMoveY(0.5f, time).SetEase(Ease.InCubic))
            .SetLoops(-1);

        if (shadow)
        {
            sequence2 = DOTween.Sequence()
                .Append(shadow.transform.DOScale(new Vector3(0.3f, 1, 0.3f), time).SetEase(Ease.OutCubic))
                .Append(shadow.transform.DOScale(new Vector3(1f, 1, 1f), time).SetEase(Ease.InCubic))
                .SetLoops(-1);
        }
    }

    private void OnEnable()
    {
        sequence.Restart();
        if (shadow)
        {
            sequence2.Restart();
        }
    }

    private void OnDisable()
    {
        sequence.Pause();
        if (shadow)
        {
            sequence2.Pause();
        }
    }
}