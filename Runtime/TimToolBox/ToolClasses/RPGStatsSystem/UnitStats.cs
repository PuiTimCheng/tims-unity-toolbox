using System;
using System.Collections.Generic;

public class UnitAttribute
{
    public enum AttributeType
    {
        Health,
        HealthRegen,
        PhysicPower,
        PhysicDefense,
        PhysicPenetration,
        MagicPower,
        MagicDefense,
        MagicPenetration,
        CritChance,
        CritDamage,
        CritResist,
        AttackSpeed,
        AbilityHaste,
        MovementSpeed,
        Accuracy,
        Dodge,
    }

    [Serializable]
    public class AttributeConfig
    {
        public float health = 100;
        public float healthRegen = 1;
        public float physicPower = 1;
        public float magicPower = 1;
        public float physicDefense = 0;
        public float magicDefense = 0;
        public float physicPenetration = 0;
        public float magicPenetration = 0;
        public float critChance = 0;
        public float critDamage = 0;
        public float critResist = 0;
        public float attackSpeed = 0;
        public float abilityHaste = 0;
        public float movementSpeed = 0;
        public float accuracy = 0;
        public float dodge = 0;
    }
    
    private Dictionary<AttributeType, Stats> _attributes = new Dictionary<AttributeType, Stats>();

    public UnitAttribute(AttributeConfig config)
    {
        _attributes.Add(AttributeType.Health, new Stats(config.health));
        _attributes.Add(AttributeType.HealthRegen, new Stats(config.healthRegen));
        _attributes.Add(AttributeType.PhysicPower, new Stats(config.physicPower));
        _attributes.Add(AttributeType.MagicPower, new Stats(config.magicPower));
        _attributes.Add(AttributeType.PhysicDefense, new Stats(config.physicDefense));
        _attributes.Add(AttributeType.MagicDefense, new Stats(config.magicDefense));
        _attributes.Add(AttributeType.PhysicPenetration, new Stats(config.physicPenetration));
        _attributes.Add(AttributeType.MagicPenetration, new Stats(config.magicPenetration));
        _attributes.Add(AttributeType.CritChance, new Stats(config.critChance));
        _attributes.Add(AttributeType.CritDamage, new Stats(config.critDamage));
        _attributes.Add(AttributeType.CritResist, new Stats(config.critResist));
        _attributes.Add(AttributeType.AttackSpeed, new Stats(config.attackSpeed));
        _attributes.Add(AttributeType.AbilityHaste, new Stats(config.abilityHaste));
        _attributes.Add(AttributeType.MovementSpeed, new Stats(config.movementSpeed));
        _attributes.Add(AttributeType.Accuracy, new Stats(config.accuracy));
        _attributes.Add(AttributeType.Dodge, new Stats(config.dodge));
    }
    
    public float GetAttributeValue(AttributeType type)
    {
        return _attributes[type].GetValue();
    }
}
