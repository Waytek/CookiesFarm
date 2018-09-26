using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookieGame : MonoBehaviour {
    CookieServer game;
    public FarmType testFarm;
    public Text cookieText;
    public Text debugText;
    public Text pingText;

    public ObserverGameState currentGameState = new ObserverGameState();
    public List<GameState> gameStates = new List<GameState>();
    // Use this for initialization
    void Start () {
        game = new CookieServer();        
        game.onGameStateChanged += SyncServer;
        game.CookiesGameServerStart();
        gameStates.Add(new GameState());
        currentGameState.onCookieNumChanged += OnUpdateCookieNum;
        currentGameState.onFarmsChanged += OnFarmChanged;
        currentGameState.UpdateGameState(gameStates[gameStates.Count - 1]);
    }
    void OnFarmChanged(List<Farm> farms)
    {
//        Debug.LogError("OnFarmChanged" + farms.Count);
    }


    void SyncServer(GameState serverState)
    {

        GameState newState = GameSmooth.SmoothState(gameStates, serverState, Time.deltaTime);
        gameStates.Add(newState);
        currentGameState.UpdateGameState(gameStates[gameStates.Count - 1]);
        Debug.LogError(newState.farms.Count);
    }
	// Update is called once per frame
	public void Click()
    {
        
        Command commandClick = new Command(Command.CommandType.CookieClick);
        GameState newState = new GameState(gameStates[gameStates.Count - 1]);        
        commandClick.ApplyCommand(newState, Time.deltaTime);
        newState.processedCommand.Add(commandClick);
        gameStates.Add(newState);
        currentGameState.UpdateGameState(gameStates[gameStates.Count - 1]);
        Sender.SendOnServer(game, commandClick);
    }
    public void BuildFarm(Farm farm)
    {

        Command buildFarm = new Command(Command.CommandType.BuildFarm, farm);
        GameState newState = new GameState(gameStates[gameStates.Count - 1]);
        buildFarm.ApplyCommand(newState, Time.deltaTime);
        newState.processedCommand.Add(buildFarm);
        gameStates.Add(newState);
        currentGameState.UpdateGameState(gameStates[gameStates.Count - 1]);
        Sender.SendOnServer(game, buildFarm);
    }
    public void TestBuidFarmClick()
    {
        Farm farm = new Farm(testFarm, currentGameState.GetFarms().Count);
        BuildFarm(farm);
    }
    void Update()
    {
        Command commandTick = new Command(Command.CommandType.ServerTick);
        GameState newState = new GameState(gameStates[gameStates.Count - 1]);
        gameStates.Add(newState);
        currentGameState.UpdateGameState(gameStates[gameStates.Count - 1]);
        commandTick.ApplyCommand(newState, Time.deltaTime*1000);
        newState.processedCommand.Add(commandTick);

        
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
            SetCookieNum(gameState.cookieNum);
            SetFarms(gameState.farms);
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
