using UnityEngine;
using System.Collections;

public class UpgradeManger : MonoBehaviour
{
    private struct Upgrade
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

    private Upgrade[] upgrades;

	void Start ()
    {
        var json = Resources.Load<TextAsset>("Upgrades").text;
        Debug.Log(json);

        
        upgrades = JsonUtility.FromJson<Upgrade[]>(json);
        Debug.Log(upgrades[0].Rank);
	}

}
