using System.Collections;
using System.Collections.Generic;
using UnityThreading;
using UnityEngine;
using System.Threading;

public class CookiesGame {
    public System.Action<GameState> onGameStateChanged = delegate { };

    bool isPlaying = true;
    
    int serverFps = 100; //ms

    Thread farmCookieThread = null;

    public List<Command> unprocessedCommand = new List<Command>();
    public List<GameState> gameStates = new List<GameState>();

    public void CookiesGameServerStart()
    {
        
        farmCookieThread = new Thread(new ThreadStart(FarmCookieThread));
        farmCookieThread.Start();
        gameStates.Add(new GameState());
    }
    void FarmCookieThread()
    {
        while (isPlaying)
        {
            lock (unprocessedCommand)
            {
                ReciveComand(new Command(Command.CommandType.ServerTick));

                GameState newState = GameSmooth.ExecuteCommands(unprocessedCommand,gameStates,serverFps);
                gameStates.Add(newState);
                Sync(newState);
            }
            Thread.Sleep(serverFps);
        }
    }
    public float DebugCookieNum()
    {
        if (gameStates.Count != 0)
            return gameStates[gameStates.Count - 1].cookieNum;
        return 0;
    }
    public void CookiesGameServerStop()
    {
        farmCookieThread.Abort();
    }
    void Sync(GameState currentState)
    {      
        Sender.SendOnClient(onGameStateChanged, currentState);
    }

    public void ReciveComand(Command command)
    {
        lock (unprocessedCommand)
        {
            unprocessedCommand.Add(command);
        }
    }







}
