using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class WeaponPreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int WeaponNumber;
    public VideoPlayer videoPlayer;
    public VideoClip[] myclip;
    public RawImage rawImage;

    public void OnPointerEnter(PointerEventData eventData)    //滑鼠移入
    {
        videoPlayer.clip = myclip[WeaponNumber];
        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 0.7f);
    }

    public void OnPointerExit(PointerEventData eventData)    //滑鼠移出
    {
        videoPlayer.clip = myclip[0];
        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 0);
    }
}
