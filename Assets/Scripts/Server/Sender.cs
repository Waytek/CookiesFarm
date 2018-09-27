using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class Sender {
    public static int ping = 1200;

    public static void SendOnClient(System.Action<GameState,System.DateTime> success, GameState gameState,List<Command> processedCommand, System.DateTime sendTime)
    {
        System.Action send = () =>
        {
            gameState = new GameState(gameState);
            Thread.Sleep(ping/2);            
            Threading.Execute(delegate 
            {
                lock (processedCommand)
                {
                    foreach (Command command in processedCommand.ToArray())
                    {
                        command.isApply = true;
                    }

                    success.Invoke(gameState, sendTime);
                }
                
            });
        };
        Thread SendThread = new Thread(new ThreadStart(send));
        SendThread.Start();
    }
    
    public static void SendOnServer(CookieServer game, Command command)
    {
        System.Action send = () =>
        {
            Thread.Sleep(ping/2);
            game.ReciveComand(command);
        };
        Thread SendThread = new Thread(new ThreadStart(send));
        SendThread.Start();
    }
}
