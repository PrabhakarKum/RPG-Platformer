using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();

    private bool _needToBeRecalculated = true;
    private float _finalValue;
    public float GetValue()
    {
        if (_needToBeRecalculated)
        {
            _finalValue = GetFinalValue();
            _needToBeRecalculated = false;
        }
        return _finalValue;
    }

    public void AddModifier(float value, string source)
    {
        var modifierToAdd = new StatModifier(value, source);
        modifiers.Add(modifierToAdd);
        _needToBeRecalculated = true;

    }
    public void RemoveModifier(string source)
    {
        modifiers.RemoveAll(modifier => modifier.source == source);
        _needToBeRecalculated = true;
    }
    private float GetFinalValue()
    {
        var totalValue = baseValue;

        foreach (var modifier in modifiers)
        {
            totalValue += modifier.value;
        }

        return totalValue;
    }

    public void SetBaseValue(float value) => baseValue = value;
}

[Serializable]
public class StatModifier
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}
