
using UnityEngine;

[CreateAssetMenu(fileName = "New Farm", menuName = "Farm", order = 51)]
public class FarmType : ScriptableObject {

    [SerializeField]
    private int basePrice;
    [SerializeField]
    private int cookiePerSecond;

    public int GetPrice(int farmCount)
    {
        float price = basePrice;
        for(int i = 0; i < farmCount; i++)
        {
            price += price * 0.09f;
            Debug.LogError("farmCount" + farmCount + " farmPrice " + price);
        }
        return (int)price; 
    }
    public int GetCookiePerSecond()
    {
        return cookiePerSecond;
    }
}
