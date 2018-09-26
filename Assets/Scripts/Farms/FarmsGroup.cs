using System.Collections.Generic;
using UnityEngine;

public class FarmsGroup: MonoBehaviour {
    public FarmType type;
    public List<Farm> farms = new List<Farm>();

    public System.Action<Farm> onBuildFarm = delegate { };

    public void BuildFarm()
    {
        Farm newFarm = new Farm(type, farms.Count);
        farms.Add(newFarm);
        onBuildFarm.Invoke(newFarm);
    }
}
