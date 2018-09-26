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
           // return;
        }
        currentGameState = GameSmooth.SmoothState(processedCommand, serverState, sendTime, Time.deltaTime);
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
        Sender.ping = Random.Range(100, 1200);
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
    }
    public class ObserverGameState{
        float cookieNum;
        public System.Action<float> onCookieNumChanged = delegate { };
        List<Farm> farms = new List<Farm>();
        public System.Action<List<Farm>> onFarmsChanged = delegate { };
        //float cookiePerClick;
        
        public void UpdateGameState(GameState gameState)
        {
            SetCookieNum(gameState.GetCookieNum());
            SetFarms(gameState.GetFarms());
        }
        public float GetCookieNum()
        {
            return cookieNum;
        }
        public void SetCookieNum(float newCookieNum)
        {
            if(cookieNum != newCookieNum)
            {
                cookieNum = newCookieNum;
                onCookieNumChanged.Invoke(cookieNum);
            }
        }
        public List<Farm> GetFarms()
        {
            return farms;
        }
        public void SetFarms(List<Farm> newFarms)
        {
            //Debug.LogError("farmsCount " + farms.GetHashCode() + " newFarmsCount " + newFarms.GetHashCode());
//            var list3 = newFarms.Except(farms).ToList();
           // Debug.LogError(newFarms.Count);
            if (newFarms.Count != farms.Count)
            {
                farms.Clear();
                farms = newFarms;
                onFarmsChanged.Invoke(farms);
            }
        }
    }
}
