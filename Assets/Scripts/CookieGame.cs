using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookieGame : MonoBehaviour {
    CookieServer game;
    public List<FarmType> testFarms = new List<FarmType>();
    public Text cookieText;
    public Text debugText;
    public Text pingText;

    //public ObserverGameState currentGameState = new ObserverGameState();
    //public List<GameState> gameStates = new List<GameState>();
    public GameState currentGameState = new GameState();
    public List<Command> processedCommand = new List<Command>();
    // Use this for initialization
    void Start () {
        PlayerPrefs.DeleteAll();
       /* List<int> test = new List<int>();
        test.Add(1);
        test.Add(2);
        test.RemoveRange(0, 0);
        Debug.LogError("test " + test.Count);
        foreach(int i in test)
        {
            Debug.LogError("testIn " + i);
        }*/
        game = new CookieServer();        
        game.onServerStateChanged += SyncServer;
        game.CookiesGameServerStart();
        //gameStates.Add(new GameState());
        //currentGameState.onCookieNumChanged += OnUpdateCookieNum;
        //currentGameState.onFarmsChanged += OnFarmChanged;
        //currentGameState.UpdateGameState(gameStates[gameStates.Count - 1]);
    }
    void OnFarmChanged(List<Farm> farms)
    {
//        Debug.LogError("OnFarmChanged" + farms.Count);
    }


    void SyncServer(GameState serverState,System.DateTime sendTime)
    {
        GameState serverStatenoLag = GameSmooth.SmoothState(processedCommand, serverState, sendTime, Time.deltaTime);
        if (Mathf.Abs(serverStatenoLag.GetCookieNum() - currentGameState.GetCookieNum()) < 10)
        {
            return;
        }
        if(Mathf.Abs(serverStatenoLag.GetCookiePerSecond() - currentGameState.GetCookiePerSecond()) < 2)
        {
            return;
        }
        currentGameState = serverStatenoLag;
        //gameStates.Add(newState);
        //currentGameState.UpdateGameState(gameStates[gameStates.Count - 1]);
        //Debug.LogError(newState.farms.Count);
    }
	// Update is called once per frame
	public void Click()
    {
        
        Command commandClick = new Command(Command.CommandType.CookieClick);
        //GameState newState = new GameState(gameStates[gameStates.Count - 1]);        
        commandClick.ApplyCommand(currentGameState, Time.deltaTime);
        processedCommand.Add(commandClick);
        //newState.processedCommand.Add(commandClick);
        //gameStates.Add(newState);
        //currentGameState.UpdateGameState(gameStates[gameStates.Count - 1]);
        Sender.SendOnServer(game, commandClick);
    }
    public void BuildFarm(Farm farm)
    {

        Command buildFarm = new Command(Command.CommandType.BuildFarm, farm);
        //GameState newState = new GameState(gameStates[gameStates.Count - 1]);
        buildFarm.ApplyCommand(currentGameState, Time.deltaTime);
        processedCommand.Add(buildFarm);
        //gameStates.Add(newState);
        //currentGameState.UpdateGameState(gameStates[gameStates.Count - 1]);
        Sender.SendOnServer(game, buildFarm);
    }
    public void TestBuidFarmClick(int numFarm)
    {
        int typeFarmsCount = 0;
        foreach (Farm f in currentGameState.GetFarms())
        {
            if (f.type == testFarms[numFarm])
                typeFarmsCount++;
        }
    
        Farm farm = new Farm(testFarms[numFarm], typeFarmsCount);
        BuildFarm(farm);
    }
    void Update()
    {
        //Sender.ping = Random.Range(100, 1200);
        Sender.ping = Random.Range(200, 1200);
        Command commandTick = new Command(Command.CommandType.ServerTick,Time.deltaTime);
        commandTick.ApplyCommand(currentGameState, Time.deltaTime*1000);        
        processedCommand.Add(commandTick);
        Sender.SendOnServer(game, commandTick);
        //Click();
        //Debug.LogError(c)
        //currentGameState.cookieNum += currentGameState.cookiePerSecond * Time.deltaTime;
        cookieText.text = ((int)currentGameState.GetCookieNum()).ToString();
        debugText.text = ((int)game.DebugCookieNum()).ToString();

    }
    void OnUpdateCookieNum(float cookieNum)
    {
        cookieText.text = ((int)cookieNum).ToString();
        //Debug.LogError("OnUpdateCookieNum");
    }
    void OnDestroy()
    {
        game.CookiesGameServerStop();
        string save = JsonUtility.ToJson(currentGameState);
        PlayerPrefs.SetString("save", save);
        PlayerPrefs.Save();
    }
}
