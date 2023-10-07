using System;
using UnityEngine;
using NaughtyAttributes;

namespace _Kittens__Kitchen_.Editor.Production.Scripts.Upgrades.Voilt
{
    public enum VoiltTypes
    {
        Tips,
        BagInvestor,
        WildPig,
        SuitCase,
        Investments
    }
    
    public abstract class Voilt : MonoBehaviour
    {
        protected VoiltData VoiltData;
        protected VoiltView VoiltView;
        protected VoiltTypes Type;

        public abstract void Initialize(VoiltTypes type, VoiltView voiltView, VoiltData voiltData);
        
        protected void Setup(VoiltTypes type, VoiltView voiltView, VoiltData voiltData)
        {
            Type = type;
            VoiltView = voiltView;
            VoiltData = voiltData;
        
            Debug.Log($"Its {Type} voilt upgrade:", gameObject);
        }
    }
    
    public class Tips : Voilt
    {
        private float _extraMoney;
        
        public override void Initialize(VoiltTypes type, VoiltView voiltView, VoiltData voiltData)
        {
            Type = type;
            VoiltView = voiltView;
            VoiltData = voiltData;
            Setup(Type, VoiltView, VoiltData);

            if (Type == VoiltTypes.Tips)
            {
                _extraMoney = VoiltData.Multiplier;
            }
        }

        public void LeaveTips(float amount)
        {
            _extraMoney *= amount;
        }
    }

    public class BagInvestor : Voilt
    {
        public override void Initialize(VoiltTypes type, VoiltView voiltView, VoiltData voiltData)
        {
            Type = type;
            VoiltView = voiltView;
            VoiltData = voiltData;
            Setup(Type, VoiltView, VoiltData);
        }
    }

    public class WildPig : Voilt
    {
        public override void Initialize(VoiltTypes type, VoiltView voiltView, VoiltData voiltData)
        {
            Type = type;
            VoiltView = voiltView;
            VoiltData = voiltData;
            Setup(Type, VoiltView, VoiltData);
        }
    }

    public class SuitCase : Voilt
    {
        public override void Initialize(VoiltTypes type, VoiltView voiltView, VoiltData voiltData)
        {
            Type = type;
            VoiltView = voiltView;
            VoiltData = voiltData;
            Setup(Type, VoiltView, VoiltData);
        }
    }

    public class Investments : Voilt
    {
        public override void Initialize(VoiltTypes type, VoiltView voiltView, VoiltData voiltData)
        {
            Type = type;
            VoiltView = voiltView;
            VoiltData = voiltData;
            Setup(Type, VoiltView, VoiltData);
        }
    }
}