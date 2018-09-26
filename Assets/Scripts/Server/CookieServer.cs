using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CookieServer {
    public System.Action<GameState,System.DateTime> onServerStateChanged = delegate { };

    bool isPlaying = true;
    
    int serverFps = 100; //ms

    Thread farmCookieThread = null;

    public List<Command> unprocessedCommand = new List<Command>();
    public List<Command> processedCommand = new List<Command>();
//    public List<GameState> gameStates = new List<GameState>();
    public GameState currentGameState = new GameState();

    public void CookiesGameServerStart()
    {

        string save = PlayerPrefs.GetString("save");
        if (!string.IsNullOrEmpty(save))
        {
            Debug.Log(save);
            currentGameState = new GameState(JsonUtility.FromJson<GameState>(save));
        }
        farmCookieThread = new Thread(new ThreadStart(FarmCookieThread));
        farmCookieThread.Start();
//        gameStates.Add(new GameState());
    }
    void FarmCookieThread()
    {
        while (isPlaying)
        {
            lock (unprocessedCommand)
            {
                if (unprocessedCommand.Count > 0)
                {
                    System.DateTime sendTime;
                    currentGameState = GameSmooth.ExecuteCommands(unprocessedCommand, processedCommand, currentGameState, serverFps, out sendTime);
                    Sync(currentGameState, sendTime);
                }                
            }
            Thread.Sleep(serverFps);
        }
    }
    public float DebugCookieNum()
    {
        return currentGameState.GetCookieNum();
    }
    public void CookiesGameServerStop()
    {
        farmCookieThread.Abort();
    }
    void Sync(GameState currentState, System.DateTime sendTime)
    {
        Sender.SendOnClient(onServerStateChanged, currentState,processedCommand, sendTime);

    }

    public void ReciveComand(Command command)
    {
        lock (unprocessedCommand)
        {
            unprocessedCommand.Add(command);
        }
    }







}
