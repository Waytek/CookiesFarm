using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookieNum : MonoBehaviour {
    Text text;
    public CookieGame game;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        game.gameStateChanged += UpdateCookieNum;
    }
	
	// Update is called once per frame
	void UpdateCookieNum (GameState gameState) {
        text.text = "Cookie " + FormatString.FormatNumber((int)gameState.GetCookieNum());
	}
}
