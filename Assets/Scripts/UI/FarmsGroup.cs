using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmsGroup: MonoBehaviour {
    Button farmGroupBtn;
    public Text coockiePerSecondText;
    public CookieGame game;
    public FarmType farmType;
    public List<GameObject> farmIcons = new List<GameObject>();



    // Use this for initialization
    void Start()
    {
       
        farmGroupBtn = GetComponent<Button>();
        game.gameStateChanged += UpdateFarmGroup;
    }
    public void UpdateFarmGroup(GameState game)
    {
        int farmsCount = Mathf.Min(farmIcons.Count, game.GetFarmCount(farmType));
        for(int i = 0; i< farmIcons.Count; i++)
        {
            if (i < farmsCount)
                farmIcons[i].SetActive(true);
            else
                farmIcons[i].SetActive(false);
        }
        coockiePerSecondText.text = (farmType.GetCookiePerSecond() * game.GetFarmCount(farmType)).ToString();
    }
}
