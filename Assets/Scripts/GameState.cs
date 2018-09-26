using System.Collections.Generic;

[System.Serializable]
public class GameState
{
    public float cookieNum;
    public List<Farm> farms = new List<Farm>();
    
    public float cookiePerClick;
    public System.DateTime time;
    public List<Command> processedCommand = new List<Command>();
    public GameState(float cookieNum, float cookiePerSec, float cookiePerClick)
    {
        this.cookieNum = cookieNum;
        this.cookiePerClick = cookiePerClick;
        time = System.DateTime.Now;
    }
    public GameState(GameState gameState)
    {
        cookieNum = gameState.cookieNum;
        if (farms != gameState.farms)
        {
            ChangeCookiePerSec();
        }
        farms = gameState.farms;
        cookiePerClick = gameState.cookiePerClick;
        time = System.DateTime.Now;
    }
    public GameState()          //стартовые значения
    {
        cookieNum = 0;
        cookiePerClick = 1;
        time = System.DateTime.Now;
    }
    public void Rewrite(GameState previosState, float stateTick)
    {
        cookieNum = previosState.cookieNum;
        cookiePerClick = previosState.cookiePerClick;
        if(farms != previosState.farms)
        {
            ChangeCookiePerSec();
        }
        farms = previosState.farms;
        foreach(Command command in processedCommand)
        {
            command.ApplyCommand(this, stateTick);
        }
    }
    private float cookiePerSecond;
    void ChangeCookiePerSec() {
        float newCookiePerSecond = 0;
        farms.ForEach((Farm farm) => cookiePerSecond += farm.GetCookiePerSecond());
        cookiePerSecond = newCookiePerSecond;
    }
    public float GetCookiePerSecond()
    {
        return cookiePerSecond;
    }

}
