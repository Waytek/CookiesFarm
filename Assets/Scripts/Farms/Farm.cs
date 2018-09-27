using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Farm {
    List<Worker> workers = new List<Worker>();
    public FarmType type;
    int workersCount = 20;
    int price;
    public float cookiePerSecond;
    public Farm(FarmType type, int farmCount)
    {
        this.type = type;
        for (int workerNum = 0; workerNum < workersCount; workerNum++)
        {
            Worker worker = new Worker();
            workers.Add(worker);
        }

        price = type.GetPrice(farmCount);
        cookiePerSecond = type.GetCookiePerSecond();
    }
    public float GetCookiePerSecond()
    {
        return cookiePerSecond;
    }
    public int GetPrice()
    {
        return price;
    }
	
}
