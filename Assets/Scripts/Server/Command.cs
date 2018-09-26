
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
        BuildFarm
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
                state.cookieNum += state.GetCookiePerSecond() * ((float)parametr);
                break;
            case Command.CommandType.CookieClick:
                state.SetCookieNum(state.GetCookieNum() + state.GetCookiePerClick());
                break;
            case Command.CommandType.BuildFarm:
                Farm newFarm = (Farm)parametr;
                if (state.GetCookieNum() >= newFarm.GetPrice())
                {
                    state.SetCookieNum(state.GetCookieNum() - newFarm.GetPrice());
                    state.AddFarm(newFarm);
                };
                break;
            default:

                break;
        }
    }
    public void DeclyneCommand(GameState state, float stateTick)
    {
        switch (commandType)
        {
            case Command.CommandType.ServerTick:
                state.cookieNum -= state.GetCookiePerSecond() * ((float)parametr);
                break;
            case Command.CommandType.CookieClick:
                state.SetCookieNum(state.GetCookieNum() - state.GetCookiePerClick());
                break;
            case Command.CommandType.BuildFarm:
                state.SetCookieNum(state.GetCookieNum() + ((Farm)parametr).GetPrice());
                state.RemoveFarm((Farm)parametr);
                break;
            default:
                break;
        }
    }
}