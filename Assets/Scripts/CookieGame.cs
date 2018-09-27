using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookieGame : MonoBehaviour {
    CookieServer server;
    public Text cookieText;
    public Text debugText;
    public Text pingText;
    public CookieClickBtn cookieClickBtn;

    public System.Action<GameState> gameStateChanged = delegate { };
    GameState currentGameState = new GameState();

    public List<Command> processedCommand = new List<Command>();

    void Start () {
        //PlayerPrefs.DeleteAll();
        server = new CookieServer();        
        server.onServerStateChanged += SyncServer;
        server.CookiesGameServerStart();

        cookieClickBtn.onClick += Click;

    }


    void SyncServer(GameState serverState,System.DateTime sendTime)
    {
        GameState serverStatenoLag = GameSmooth.SmoothState(processedCommand, serverState, sendTime, Time.deltaTime);
        if (Mathf.Abs(serverStatenoLag.GetCookieNum() - currentGameState.GetCookieNum()) < serverStatenoLag.GetCookiePerSecond()*2)
        {
            return;
        }
        /*if (Mathf.Abs(serverStatenoLag.GetCookieNum() - currentGameState.GetCookieNum()) < 10)
        {
            return;
        }*/
        if(Mathf.Abs(serverStatenoLag.GetFarms().Count - currentGameState.GetFarms().Count) < 3)
        {
            return;
        }
        currentGameState = serverStatenoLag;
        gameStateChanged.Invoke(currentGameState);
    }
	public void Click()
    {
        
        Command commandClick = new Command(Command.CommandType.CookieClick);
        commandClick.ApplyCommand(currentGameState, Time.deltaTime);
        processedCommand.Add(commandClick);
        gameStateChanged.Invoke(currentGameState);
        Sender.SendOnServer(server, commandClick);
    }

    public void BuidFarm(FarmType farmType)
    {    
    
        Farm farm = new Farm(farmType, currentGameState.GetFarmCount(farmType));
        Command buildFarm = new Command(Command.CommandType.BuildFarm, farm);
        buildFarm.ApplyCommand(currentGameState, Time.deltaTime);
        processedCommand.Add(buildFarm);
        gameStateChanged.Invoke(currentGameState);
        Sender.SendOnServer(server, buildFarm);
    }
    void Update()
    {
        //Sender.ping = Random.Range(100, 1200);
        Command commandTick = new Command(Command.CommandType.ServerTick,Time.deltaTime);
        commandTick.ApplyCommand(currentGameState, Time.deltaTime*1000);        
        processedCommand.Add(commandTick);
        gameStateChanged.Invoke(currentGameState);
        Sender.SendOnServer(server, commandTick);
        debugText.text = ((int)server.DebugCookieNum()).ToString();

    }
    void OnDestroy()
    {
        server.CookiesGameServerStop();
        string save = Base64.Base64Encode(JsonUtility.ToJson(currentGameState));
        PlayerPrefs.SetString("save", save);
        PlayerPrefs.Save();
    }
}
