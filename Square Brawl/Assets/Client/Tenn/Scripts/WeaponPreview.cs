using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class WeaponPreview : MonoBehaviour
{
    public int WeaponNumber;
    public VideoPlayer videoPlayer;
    public VideoClip[] myclip;
    public Image[] icon;
    public TextMeshProUGUI[] weaponName;
    public RawImage rawImage;
    public TextMeshProUGUI text;

    private Sequence sequence;

    private void PlayPreview()
    {
        videoPlayer.clip = myclip[WeaponNumber];

        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(rawImage
                    .DOColor(new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 1f), 0.3f)
                    .SetEase(Ease.OutCirc))
                .Join(icon[WeaponNumber]
                    .DOColor(new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 1f), 0.3f)
                    .SetEase(Ease.OutCirc))
                .Join(weaponName[WeaponNumber]
                    .DOColor(new Color(1, 1, 1, 1f), 0.3f)
                    .SetEase(Ease.OutCirc));
    }

    private void ExitPreview()
    {
        videoPlayer.clip = myclip[0];

        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(rawImage
                    .DOColor(new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 0f), 0.3f)
                    .SetEase(Ease.OutCirc))
                .Join(icon[WeaponNumber]
                    .DOColor(new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 0f), 0.3f)
                    .SetEase(Ease.OutCirc))
                .Join(weaponName[WeaponNumber]
                    .DOColor(new Color(1, 1, 1, 0f), 0.3f)
                    .SetEase(Ease.OutCirc));
    }
}
