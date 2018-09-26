using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState
{
    public float cookieNum;
    public List<Farm> farms = new List<Farm>();
    
    float cookiePerClick;

    public GameState(float cookieNum, float cookiePerSec, float cookiePerClick)
    {
        this.cookieNum = cookieNum;
        this.cookiePerClick = cookiePerClick;
    }
    public GameState(GameState gameState)
    {
        cookieNum = gameState.cookieNum;
        if (farms != gameState.farms)
        {
            farms = new List<Farm>(gameState.farms);
            ChangeCookiePerSec();
        }
        cookiePerClick = gameState.cookiePerClick;
    }
    public GameState()          //стартовые значения
    {      
            cookieNum = 0;
            cookiePerClick = 1;
      
    }

    public float GetCookieNum()
    {
        return cookieNum;
    }
    public void SetCookieNum(float cookieNum)
    {
        this.cookieNum = cookieNum;
    }

    public float GetCookiePerClick()
    {
        return cookiePerClick;
    }
    public void SetCookieCookiePerClick(float cookiePerClick)
    {
        this.cookiePerClick = cookiePerClick;
    }


    public void AddFarm(Farm farm)
    {
        farms.Add(farm);
        ChangeCookiePerSec();
    }
    public void RemoveFarm(Farm farm)
    {
        farms.Remove(farm);
        ChangeCookiePerSec();
    }
    public List<Farm> GetFarms()
    {
        return farms;
    }
    private float cookiePerSecond;
    void ChangeCookiePerSec() {
        float newCookiePerSecond = 0;
        farms.ForEach((Farm farm) => newCookiePerSecond += farm.GetCookiePerSecond());
        cookiePerSecond = newCookiePerSecond;
    }
    public float GetCookiePerSecond()
    {
        return cookiePerSecond;
    }

}
