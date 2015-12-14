using UnityEngine;
using System.Collections;
using System;

public class UpgradeManger : MonoBehaviour
{
    [Serializable]
    private struct UpgradeList
    {
        public Upgrade[] Upgrades;
    }

    [Serializable]
    public struct Upgrade
    {
        public string Name;
        public string ToolTip;
        public int WeaponType;

        public int Rank;
        public int RequiredExp;

        public int PrimaryDamage;
        public float PrimaryFireRate;
        public float PrimaryHeatUp;
        public float PrimaryCooldown;

        public int SecodaryDamage;
        public float SecondaryChargeSpeed;
    }

    [SerializeField]
    private Upgrade[] upgrades;

	public static Upgrade[] GetUpdate(int rank)
    {
        var json = Resources.Load<TextAsset>("Upgrades").text;
        var upgradeList = JsonUtility.FromJson<UpgradeList>(json);

        Upgrade[] foundUpgrades = new Upgrade[2];
        int counter = 0;

        foreach (var upgrade in upgradeList.Upgrades)
        {
            if (upgrade.Rank == rank)
            {
                foundUpgrades[counter] = upgrade;
                counter++;
            }
            if (counter > 2) { break; }
        }

        return foundUpgrades;
	}

    public static float[] GetExpIntervals()
    {
        var json = Resources.Load<TextAsset>("Upgrades").text;
        var upgradeList = JsonUtility.FromJson<UpgradeList>(json);

        float[] foundIntervals = new float[upgradeList.Upgrades.Length/2];

        for (int i =0; i < upgradeList.Upgrades.Length; i+=2)
        {
            foundIntervals[i/2] = upgradeList.Upgrades[i].RequiredExp;
        }
        return foundIntervals;
    }
}
