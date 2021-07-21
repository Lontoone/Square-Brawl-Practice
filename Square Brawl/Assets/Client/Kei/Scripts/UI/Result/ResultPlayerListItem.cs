using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResultPlayerListItem : MonoBehaviour
{
    public Player player;
    public Image playerColor;
    public Text playerNameText;

    public GameObject lifeContainer;
    private List<Image> lifes;

    private int winCount = 0;
    private int maxWinCount = 0;    

    public void SetUp(Player _player)
    {
        player = _player;
        playerColor.color = CustomPropertyCode.COLORS[(int)_player.CustomProperties[CustomPropertyCode.TEAM_CODE]];
        playerNameText.text = player.NickName;

        maxWinCount = lifeContainer.transform.childCount;
        for (int i = 0; i < maxWinCount; i++)
        {
            lifes.Add(lifeContainer.transform.GetChild(i).GetComponent<Image>());
        }
        winCount = 0;
    }


    public void SetWinColor() {
        if (winCount >= maxWinCount)
        {
            //Win the game
            FindObjectOfType<ResultManager>()?.EndGame();
        }
        else {
            //Win this round
            lifes[winCount].color = playerColor.color;
        }
        winCount++;

        
    }

}
