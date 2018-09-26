using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class Sender {
    public static int ping = 1000;

    public static void SendOnClient(System.Action<GameState> success, GameState gameState)
    {
        System.Action send = () =>
        {
            Thread.Sleep(ping/2);
            //Debug.LogError(gameState.time + " Now " + System.DateTime.Now);
            Threading.Execute(delegate { success.Invoke(gameState); });
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
