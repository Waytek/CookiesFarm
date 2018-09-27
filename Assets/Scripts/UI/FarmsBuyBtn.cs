using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmsBuyBtn : MonoBehaviour {
    Button buildFarmBtn;
    public CookieGame game;
    public Text priceText;
    public FarmType farmType;


    // Use this for initialization
    void Start()
    {
        buildFarmBtn = GetComponent<Button>();
        buildFarmBtn.onClick.AddListener(() => game.BuidFarm(farmType));
        game.gameStateChanged += UpdateBtn;
    }
    public void UpdateBtn(GameState game)
    {
        int price = farmType.GetPrice(game.GetFarmCount(farmType));
        buildFarmBtn.interactable = (game.GetCookieNum() >= price);
        priceText.text = FormatString.FormatNumber(price);
    }

}
