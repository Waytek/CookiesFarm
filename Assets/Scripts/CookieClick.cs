using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookieClick : MonoBehaviour {
    CookiesGame game;

    public Text cookieText;
    public Text debugText;
    public Text pingText;

    public List<GameState> gameStates = new List<GameState>();
    public List<GameState> serverGameStates = new List<GameState>();
    // Use this for initialization
    void Start () {
        game = new CookiesGame();        
        game.onGameStateChanged += SyncServer;
        game.CookiesGameServerStart();
        gameStates.Add(new GameState());   
    }
	void SyncServer(GameState serverState)
    {

        GameState newState = GameSmooth.SmoothState(gameStates, serverState, Time.deltaTime);
        gameStates.Add(newState);


    }
	// Update is called once per frame
	public void Click()
    {
        
        Command commandClick = new Command(Command.CommandType.CookieClick);
        GameState newState = new GameState(gameStates[gameStates.Count - 1]);        
        commandClick.ApplyCommand(newState, Time.deltaTime);
        newState.processedCommand.Add(commandClick);
        gameStates.Add(newState);
        Sender.SendOnServer(game, commandClick);
    }
    public void AddCPSTest(float CPS)
    {

        Command buildFactory = new Command(Command.CommandType.BuildFactory,CPS);
        GameState newState = new GameState(gameStates[gameStates.Count - 1]);
        buildFactory.ApplyCommand(newState, Time.deltaTime);
        newState.processedCommand.Add(buildFactory);
        gameStates.Add(newState);
        Sender.SendOnServer(game, buildFactory);
    }
    void Update()
    {
        Command commandTick = new Command(Command.CommandType.ServerTick);
        GameState newState = new GameState(gameStates[gameStates.Count - 1]);
        gameStates.Add(newState);
        commandTick.ApplyCommand(newState, Time.deltaTime*1000);
        newState.processedCommand.Add(commandTick);

        cookieText.text = ((int)gameStates[gameStates.Count - 1].cookieNum).ToString();
        debugText.text = game.DebugCookieNum().ToString();

    }
    void OnDestroy()
    {
        game.CookiesGameServerStop();
    }
}
