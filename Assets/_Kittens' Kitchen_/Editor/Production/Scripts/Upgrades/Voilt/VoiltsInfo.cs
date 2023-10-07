using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using NaughtyAttributes;

namespace _Kittens__Kitchen_.Editor.Production.Scripts.Upgrades.Voilt
{
    public class VoiltsInfo : ScriptableObject
    {
        [field: SerializeField]
        public VoiltData[] AllVoilts { get; private set; }

        [SerializeField] private bool showDebugParameters;

        [ShowIf("showDebugParameters")] 
        [SerializeField] private List<VoiltData> purchasedVoilts;
        public List<VoiltData> PurchasedVoilts {get => purchasedVoilts; set => purchasedVoilts = value; }

        [ShowIf("showDebugParameters")] [Button("RefreshLevels")]

        private void RefreshLevels()
        {
            foreach (var voilt in AllVoilts)
            {
                voilt.CurrentLevel = 0;
                Debug.Log("Voilts were refreshed");
            }
        }

        public float LeaveTips(VoiltData voiltTips, VoiltData voiltWildPig)
        {
            if (voiltTips.Type != VoiltTypes.Tips)
            {
                Debug.Log("is not tips"); 
                return 0;
            } 
            
            float tipsAmount;

            tipsAmount = voiltTips.Multiplier;

            if (voiltWildPig != null)
            {
                if (voiltWildPig.Type == VoiltTypes.WildPig) tipsAmount *= voiltWildPig.Multiplier;
            }

            return tipsAmount;
        }

        public float AddWildPig(VoiltData voiltWildPig)
        {
            if (voiltWildPig.Type != VoiltTypes.WildPig)
            {
                Debug.Log("is not wildPig"); 
                return 0;
            }

            float wildPigAmount;

            wildPigAmount = voiltWildPig.Multiplier;

            return wildPigAmount;
        } 

        public VoiltData ReturnVoiltOfType(VoiltTypes voiltType)
        {
            foreach (var voilt in AllVoilts)
            {
                if (voilt.Type == voiltType && voilt.IsBought()) return voilt;
            }

            Debug.Log("Returned is null type of voilt");
            return null;
        }
    }
}