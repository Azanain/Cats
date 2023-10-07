using System;
using System.Collections.Generic;
using UnityEngine;
public class ButtonSelectUpgradeRestaurantPool : MonoBehaviour
{
    [SerializeField] private ButtonSelectUpgradeRestaurant _prefab;

    [SerializeField] private Transform _contentTransform;
    [SerializeField] private int _minCapacity;
    [SerializeField] private int _maxCapacity;
    [SerializeField] private bool _autoExpand;

    [HideInInspector] public List<ButtonSelectUpgradeRestaurant> pool;

    private void OnValidate()
    {
        if (_autoExpand)
        {
            _maxCapacity = int.MaxValue;
        }
    }
    public void Initialize(int addMin, bool isFirstCreatePool)
    {
        _minCapacity = addMin;
        _maxCapacity = _minCapacity + 1;

        if(isFirstCreatePool)
            CreatePool();
    }
    private void CreatePool()
    {
        pool = new List<ButtonSelectUpgradeRestaurant>(_minCapacity);

        for (int i = 0; i < _minCapacity; i++)
            CreateAllElement();
    }
    private ButtonSelectUpgradeRestaurant CreateAllElement(bool isActiveByDefault = false)
    {
        ButtonSelectUpgradeRestaurant createdObject = null;
        createdObject = Instantiate(_prefab, _contentTransform);
        createdObject.gameObject.SetActive(false);
        pool.Add(createdObject);

        return createdObject;
    }
    public bool TryGetElement(out ButtonSelectUpgradeRestaurant element)
    {
        foreach (var item in pool)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                element = item;
                item.gameObject.SetActive(true);
                return true;
            }
        }
        element = null;
        return false;
    }
    public ButtonSelectUpgradeRestaurant GetFreeElement(Vector3 position, Quaternion rotation)
    {
        var element = GetFreeElement(position);
        element.transform.rotation = rotation;
        return element;
    }
    public ButtonSelectUpgradeRestaurant GetFreeElement(Vector3 position)
    {
        var element = GetFreeElement();
        element.transform.position = position;
        return element;
    }
    public ButtonSelectUpgradeRestaurant GetFreeElement()
    {
        if (TryGetElement(out var element))
        {
            return element;
        }
        if (_autoExpand)
        {
            return CreateAllElement(true);
        }
        if (pool.Count < _maxCapacity)
        {
            return CreateAllElement(true);
        }
        throw new Exception("Poo; is over!");
    }
}
