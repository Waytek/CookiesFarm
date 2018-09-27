using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookieClickBtn : MonoBehaviour {
    public CookieGame game;
    Button cookieBtn;
    public System.Action onClick = delegate { };
	// Use this for initialization
	void Start () {
        cookieBtn = GetComponent<Button>();
        cookieBtn.onClick.AddListener(() => game.Click());
	}

}
