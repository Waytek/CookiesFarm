using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Threading : MonoBehaviour {
    private static object sync = new object();
    private static List<System.Action> actions = new List<System.Action>();
    private static Dictionary<System.Action<object>, object> parametricActions = new Dictionary<System.Action<object>, object>();
    // Use this for initialization
    void Update()
    {
        lock (sync)
        { //обеспечиваем потокобезопасность чтения листа

            foreach (KeyValuePair<System.Action<object>, object> entry in parametricActions)
            {
                entry.Key.Invoke(entry.Value);

                // do something with entry.Value or entry.Key
            }
            parametricActions.Clear();

            while (actions.Count != 0)
            { //и исполняем все действия
                actions[0].Invoke();
                actions.RemoveAt(0);
            }

        }
    }
    public static void Execute(System.Action action)
    {
        lock (sync)
        { //обеспечиваем потокобезопасность записи в лист
            actions.Add(action);
        }
        try
        {
            Thread.Sleep(0);//усыпляем вызвавший поток
        }
        catch (ThreadInterruptedException)
        {
        }
        finally { }
    }
    public static void Execute(System.Action<object> action, object parametr)
    {
        lock (sync)
        { //обеспечиваем потокобезопасность записи в лист
            parametricActions.Add(action,parametr);
        }
        try
        {
            Thread.Sleep(0);//усыпляем вызвавший поток
        }
        catch (ThreadInterruptedException)
        {
        }
        finally { }
    }
}
