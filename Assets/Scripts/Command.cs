using UnityEngine;

[System.Serializable]
public class Command
{
    public CommandType commandType;
    public object parametr;
    public System.DateTime time;
    public enum CommandType
    {
        ServerTick,
        CookieClick,
        BuildFactory
    }
    public Command(CommandType commandType, object parametr = null)
    {
        this.commandType = commandType;
        this.time = System.DateTime.Now;
        this.parametr = parametr;
    }
    public void ApplyCommand(GameState state,float stateTick)
    {
        switch (commandType)
        {
            case Command.CommandType.ServerTick:
                state.cookieNum += state.cookiePerSec * (stateTick / 1000f);
                break;
            case Command.CommandType.CookieClick:
                state.cookieNum += state.cookiePerClick;
                break;
            case Command.CommandType.BuildFactory:
                if (state.cookieNum - 100 > 0)
                {
                    state.cookieNum -= 100;
                    state.cookiePerSec += (float)parametr;
                };
                
                break;
            default:

                break;
        }
        //state.processedCommand.Add(this);

    }
    public void DeclyneCommand(GameState state, float stateTick)
    {
        switch (commandType)
        {
            case Command.CommandType.ServerTick:
                state.cookieNum -= state.cookiePerSec * (stateTick / 1000f);
                break;
            case Command.CommandType.CookieClick:
                state.cookieNum -= state.cookiePerClick;
                break;
            case Command.CommandType.BuildFactory:
                break;
            default:
                break;
        }
    }
}