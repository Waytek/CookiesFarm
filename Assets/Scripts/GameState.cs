using System.Collections.Generic;

[System.Serializable]
public class GameState
{
    public float cookieNum;
    public float cookiePerSec;
    public float cookiePerClick;
    public System.DateTime time;
    public List<Command> processedCommand = new List<Command>();
    public GameState(float cookieNum, float cookiePerSec, float cookiePerClick)
    {
        this.cookieNum = cookieNum;
        this.cookiePerSec = cookiePerSec;
        this.cookiePerClick = cookiePerClick;
        time = System.DateTime.Now;
    }
    public GameState(GameState gameState)
    {
        cookieNum = gameState.cookieNum;
        cookiePerSec = gameState.cookiePerSec;
        cookiePerClick = gameState.cookiePerClick;
        time = System.DateTime.Now;
    }
    public GameState()          //стартовые значения
    {
        cookieNum = 0;
        cookiePerSec = 0;
        cookiePerClick = 1;
        time = System.DateTime.Now;
    }
    public void Rewrite(GameState previosState, float stateTick)
    {
        cookieNum = previosState.cookieNum;
        cookiePerClick = previosState.cookiePerClick;
        cookiePerSec = previosState.cookiePerSec;
        foreach(Command command in processedCommand)
        {
            command.ApplyCommand(this, stateTick);
        }
    }

}
