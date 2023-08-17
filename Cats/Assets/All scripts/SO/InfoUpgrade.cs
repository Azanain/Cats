using UnityEngine;

[CreateAssetMenu]
public class InfoUpgrade : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;

    [Space(5)]
    [SerializeField] private float _speed;
    [SerializeField] private float _weight;
    [SerializeField] private int _quality;
    [SerializeField] private uint _cost;

    public string Name => _name;
    public Sprite Sprite => _sprite;

    public float Speed => _speed;
    public float Weight => _weight;
    public int Quality => _quality;
    public uint Cost => _cost;
}
