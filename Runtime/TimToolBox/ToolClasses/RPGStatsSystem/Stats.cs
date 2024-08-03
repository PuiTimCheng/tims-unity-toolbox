using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public float DefaultValue { get; private set;}
    public List<Modifier> modifiers;
    
    public Stats(float defaultValue)
    {
        this.DefaultValue = defaultValue;
    }

    public float GetValue()
    {
        float totalConstant = default;
        float totalPercentage = default;
        foreach (var modifier in modifiers)
        {
            totalConstant += modifier.constant;
            totalPercentage += modifier.percentage;
        }
        return (DefaultValue + totalConstant) * (1 + totalPercentage);
    }
    
    public Modifier AddModifer(float constant = default(float), float percentage = default(float))
    {
        var modifier = new Modifier
        {
            constant = constant,
            percentage = percentage
        };
        modifiers.Add(modifier);
        return modifier;
    }
    public bool RemoveModifer(Modifier modifier)
    {
        return modifiers.Remove(modifier);
    }
    
    public class Modifier
    {
        public float constant;
        public float percentage;
    }
}

