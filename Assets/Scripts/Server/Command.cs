
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
                state.cookieNum += state.GetCookiePerSecond() * (stateTick / 1000f);
                break;
            case Command.CommandType.CookieClick:
                state.cookieNum += state.cookiePerClick;
                break;
            case Command.CommandType.BuildFarm:
                Farm newFarm = (Farm)parametr;
                //Debug.LogError(state.cookieNum + " " + newFarm.GetPrice());
                if (state.cookieNum >= newFarm.GetPrice())
                {
                    state.cookieNum -= newFarm.GetPrice();
                    state.farms.Add(newFarm);
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
                state.cookieNum -= state.GetCookiePerSecond() * (stateTick / 1000f);
                break;
            case Command.CommandType.CookieClick:
                state.cookieNum -= state.cookiePerClick;
                break;
            case Command.CommandType.BuildFarm:
                break;
            default:
                break;
        }
    }
}