using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class WeaponPreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int WeaponNumber;
    public VideoPlayer videoPlayer;
    public VideoClip[] myclip;
    public Image[] icon;
    public TextMeshProUGUI[] weaponName;
    public RawImage rawImage;
    public TextMeshProUGUI text;

    private Sequence sequence;
    private bool enterButton = false;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        /*if (eventData.button == PointerEventData.InputButton.Right && enterButton == true)
        {
            PlayPreview();
        }*/
    }

    public void OnPointerEnter(PointerEventData eventData)    //滑鼠移入
    {
        PlayPreview();
        //enterButton = true;
        //text.DOColor(new Color(0.7f, 0.7f, 0.7f, 1), 0.3f).SetEase(Ease.OutCirc);
    }

    public void OnPointerExit(PointerEventData eventData)    //滑鼠移出
    {
        ExitPreview();
        //enterButton = false;
        //text.DOColor(new Color(1, 1, 1, 0), 0.3f).SetEase(Ease.OutCirc);
        //videoPlayer.clip = myclip[0];
    }
}
