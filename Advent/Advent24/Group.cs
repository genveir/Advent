using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.Advent24
{
    public enum Affiliation { ImmuneSystem, Infection }

    class Group
    {
        public Unit unitType;
        public int originalUnits;
        public int numUnits;
        public Affiliation affiliation;

        public int EffectivePower { get { return unitType.AttackDamage * numUnits; } }

        public int DamageTo(Group group)
        {
            // The damage an attacking group deals to a defending group depends on the attacking group's
            // attack type and the defending group's immunities and weaknesses. By default, an attacking group
            // would deal damage equal to its effective power to the defending group. However, if the defending 
            // group is immune to the attacking group's attack type, the defending group instead takes no damage;
            // if the defending group is weak to the attacking group's attack type, the defending group instead 
            // takes double damage.

            var damage = EffectivePower;
            if (group.unitType.immunities.Contains(this.unitType.attackType)) damage = 0;
            else if (group.unitType.weaknesses.Contains(this.unitType.attackType)) damage *= 2;

            return damage;
        }

        public int TakeDamageFrom(Group group)
        {
            // The defending group only loses whole units from damage; damage is always dealt in such a way that it 
            // kills the most units possible, and any remaining damage to a unit that does not immediately kill it is ignored. 
            var damage = group.DamageTo(this);

            var unitsKilled = damage / unitType.hitpoints;
            numUnits -= unitsKilled;
            if (numUnits < 0) numUnits = 0;

            return unitsKilled;
        }

        public void Reset()
        {
            numUnits = originalUnits;
            unitType.boost = 0;
        }

        public void Boost(int boost)
        {
            unitType.boost = boost;
        }

        public override string ToString()
        {
            return string.Format("{0} group {1}", affiliation, unitType.initiative);
        }

        public static Group Parse(string input, Affiliation affiliation)
        {
            var words = input.Split(new char[] { ' ', ',', ';', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            
            var group = new Group();
            var unit = new Unit();
            group.unitType = unit;
            group.affiliation = affiliation;

            int cursor = 0;

            group.numUnits = int.Parse(words[cursor++]); // 2546
            group.originalUnits = group.numUnits;
            cursor += 3; // units each with
            group.unitType.hitpoints = int.Parse(words[cursor++]); // 49009
            cursor += 2; // hit points
            while (words[cursor] != "with")
            {
                if (words[cursor] == "weak")
                {
                    cursor += 2; // weak to
                    bool nextIsWeakness = true;
                    while (nextIsWeakness)
                    {
                        var weakness = Enum.Parse<DamageType>(words[cursor++]); // weakness
                        group.unitType.weaknesses.Add(weakness);
                        nextIsWeakness = !(words[cursor] == "immune" || words[cursor] == "with");
                    }
                }
                if (words[cursor] == "immune")
                {
                    cursor += 2; // immune to
                    bool nextIsImmunity = true;
                    while (nextIsImmunity)
                    {
                        var immunity = Enum.Parse<DamageType>(words[cursor++]); // weakness
                        group.unitType.immunities.Add(immunity);
                        nextIsImmunity = !(words[cursor] == "weak" || words[cursor] == "with");
                    }
                }
            }
            cursor += 5; // with an attack that does
            group.unitType.attackDamage = int.Parse(words[cursor++]); // 38
            group.unitType.attackType = Enum.Parse<DamageType>(words[cursor++]); // bludgeoning
            cursor += 3; //   damage at initiative 
            group.unitType.initiative = int.Parse(words[cursor++]); // 6

            return group;
        }
    }
}
