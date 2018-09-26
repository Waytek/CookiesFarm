using System.Collections.Generic;
using UnityEngine;

public class Farm {
    List<Worker> workers = new List<Worker>();

    int workersCount = 20;
    int price;
    float cookiePerSecond;
    public Farm(FarmType type, int farmCount)
    {
        for (int workerNum = 0; workerNum < workersCount; workerNum++)
        {
            Worker worker = new Worker();
            workers.Add(worker);
        }

        price = type.GetPrice(farmCount);
//        Debug.LogError(price);
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
