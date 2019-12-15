using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2018.Advent24
{
    public enum DamageType { none, slashing, bludgeoning, fire, cold, radiation}

    class Unit
    {
        public Unit()
        {
            weaknesses = new List<DamageType>();
            immunities = new List<DamageType>();
        }

        public int hitpoints;
        public int attackDamage { set; private get; }
        public DamageType attackType;
        public int initiative;
        public List<DamageType> weaknesses;
        public List<DamageType> immunities;

        public int boost;

        public int AttackDamage { get { return attackDamage + boost; } }
    }
}
