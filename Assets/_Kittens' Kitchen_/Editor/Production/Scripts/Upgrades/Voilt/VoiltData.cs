using System;
using System.Collections;
using System.Collections.Generic;
using _Kittens__Kitchen_.Editor.Production.Scripts.Upgrades.Voilt;
using NaughtyAttributes;
using UnityEngine;

public class VoiltData : ScriptableObject
{
    [Header("Data")]
    [ShowAssetPreview] public Sprite Icon;
    [SerializeField] private VoiltTypes type;
    [SerializeField] private int maxLevel;
    [SerializeField] private int cost;
    [SerializeField] private string label;
    [SerializeField] private string description;

    [Header("Parameters")] 
    [SerializeField] private float multiplier;

    [Header("Debug")] 
    [SerializeField] private bool showDebugParameters;
    [ShowIf("showDebugParameters")] [SerializeField] private int currentLevel;

    public float Multiplier => multiplier;
    public VoiltTypes Type => type;
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public int MaxLevel => maxLevel;
    public int Cost => cost;
    public string Label => label;
    public string Description => description;

    public bool IsBought()
    {
        return CurrentLevel > 0;
    }

    public bool IsMaxLevel()
    {
        return CurrentLevel >= MaxLevel;
    }
}
