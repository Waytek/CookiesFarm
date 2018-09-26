using System.Collections.Generic;
using UnityEngine;

public class GameSmooth {

    public static GameState ExecuteCommands(List<Command> unprocessedCommand, List<Command> processedCommand, GameState currentState, float stateTick, out System.DateTime sendTime)
    {

        /*Сервер:
        Получить состояние в момент самой старой команды,
        для этого берём выполненные команды, оставляем только те что были выполнены после самой старой невыполненной команда, остальные удаляем
        берём теущий гейм стэйт и откатываем с него выполненные команды
        к получившемуся состоянию применяем выполненные и невыполненные команды, отсортированные по дате по возрастанию
        возвращаем получившееся состояние
        */
        unprocessedCommand.Sort((x, y) => System.DateTime.Compare(x.time, y.time));
        sendTime = unprocessedCommand[unprocessedCommand.Count - 1].time;
        /*if (processedCommand.Count > 0)
        {
            int numToRemove = processedCommand.FindLastIndex((Command command) => command.time < unprocessedCommand[0].time);
            if (numToRemove != -1)
                processedCommand.RemoveRange(0, numToRemove + 1);
        }
        foreach (Command command in processedCommand)
        {
            command.DeclyneCommand(currentState, stateTick);
        }
        processedCommand.AddRange(unprocessedCommand);
        unprocessedCommand.Clear();
        foreach (Command command in processedCommand)
        {
            command.ApplyCommand(currentState, stateTick);
        }*/
        processedCommand.AddRange(unprocessedCommand);
        foreach (Command command in unprocessedCommand)
        {
            command.ApplyCommand(currentState, stateTick);
        }
        unprocessedCommand.Clear();
        return currentState;


    }
    public static GameState SmoothState(List<Command> processedCommand, GameState changedGameState, System.DateTime serverStateTime, float stateTick)
    {
        /*Клиент:
        Получаем с сервера состояние и время его отправки
        из массива выполненных команд удаляем те, что старше пришедшего состояния
        оставшиеся применяем к состоянию
        
        Примечание: 
        - до применение сглаживания стоит сравнить пришедшее состояние с текущим состоянием клиента и принять решение - нужно сглаживать или нет, 
        что бы уменьшить нагрузку на клиент;
        сравнить состояние лучше в классе игры
        */

        /*int numToRemove = processedCommand.FindLastIndex((Command command) => command.time <= serverStateTime);
        if(numToRemove == -1)
        {
            //processedCommand.Clear();
        }
        processedCommand.RemoveRange(0, numToRemove+1);*/
        processedCommand.RemoveAll((Command command) => command.isApply);
        foreach (Command command in processedCommand)
        {
            command.ApplyCommand(changedGameState, stateTick);
        }
        return changedGameState;
    }
}
