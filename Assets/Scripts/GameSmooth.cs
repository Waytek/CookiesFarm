using System.Collections.Generic;
using UnityEngine;

public class GameSmooth {

    public static GameState ExecuteCommands(List<Command> unprocessedCommand, List<GameState> gameStates, float stateTick)
    {
        unprocessedCommand.Sort((x, y) => System.DateTime.Compare(x.time, y.time));
        if(unprocessedCommand.Count == 0)
        {
            return new GameState(gameStates[gameStates.Count - 1]);
        }
        int actualStateNum = gameStates.FindIndex((GameState state) => state.time > unprocessedCommand[0].time);
        if (actualStateNum > 0)
        {
            gameStates.RemoveRange(0, actualStateNum - 1);
        }
        foreach (Command command in unprocessedCommand)
        {
            command.ApplyCommand(gameStates[0], stateTick);
            gameStates[0].processedCommand.Add(command);
        }
        unprocessedCommand.Clear();
        for (int i = 1; i < gameStates.Count; i++)
        {                           
            gameStates[i].Rewrite(gameStates[i - 1], stateTick);
        }
        return new GameState(gameStates[gameStates.Count - 1]);
    }
    public static GameState SmoothState(List<GameState> gameStates, GameState changedState, float stateTick)
    {
        int stateToChangeNum = gameStates.FindIndex((GameState state) => state.time > changedState.time);
        if (stateToChangeNum > 0)
        {
            gameStates.RemoveRange(0, stateToChangeNum - 1);
            return new GameState(gameStates[gameStates.Count - 1]);
        }


        for (int i = 0; i < gameStates.Count; i++)
        {
            if (i == 0)
            {
                gameStates[i].Rewrite(changedState, stateTick);
            }
            else
            {
                gameStates[i].Rewrite(gameStates[i - 1], stateTick);
            }
        }      
        return new GameState(gameStates[gameStates.Count - 1]);            
    }
}
