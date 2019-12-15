using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2018.Advent24
{
    class Fight
    {
        public List<Group> immuneSystemArmy;
        public List<Group> infectionArmy;

        public Fight(List<Group> immuneSystem, List<Group> infection)
        {
            this.immuneSystemArmy = immuneSystem;
            this.infectionArmy = infection;
        }

        public void Reset()
        {
            foreach (var group in immuneSystemArmy) group.Reset();
            foreach (var group in infectionArmy) group.Reset();
        }

        public void Boost(int boost)
        {
            foreach (var group in immuneSystemArmy) group.Boost(boost);
        }

        public Affiliation Winner;
        public bool DoRound()
        {
            var targets = DoTargetSelection();
            int numKilledThisRound = DoAttack(targets);

            if (numKilledThisRound == 0 || immuneSystemArmy.Sum(g => g.numUnits) == 0) { Winner = Affiliation.Infection; return false; }
            if (infectionArmy.Sum(g => g.numUnits) == 0) { Winner = Affiliation.ImmuneSystem; return false; }

            return true;
        }

        public Dictionary<Group, Group> DoTargetSelection()
        {
            // During the target selection phase, each group attempts to choose one target.In decreasing order of effective power, 
            //groups choose their targets; in a tie, the group with the higher initiative chooses first.
            var allGroupsWithUnits = immuneSystemArmy
                 .Union(infectionArmy)
                 .Where(g => g.numUnits > 0)
                 .OrderByDescending(g => g.EffectivePower)
                 .ThenByDescending(g => g.unitType.initiative)
                 .ToList();

            var targets = new Dictionary<Group, Group>();
            var alreadyPicked = new HashSet<Group>();
            foreach (var group in allGroupsWithUnits)
            {
                // The attacking group chooses to target the group in the enemy army to which it would deal the most damage
                // (after accounting for weaknesses and immunities, but not accounting for whether the defending group has 
                // enough units to actually receive all of that damage).

                // If an attacking group is considering two defending groups to which it would deal equal damage, 
                // it chooses to target the defending group with the largest effective power; if there is still a tie, it 
                // chooses the defending group with the highest initiative.If it cannot deal any defending groups damage, it
                // does not choose a target.Defending groups can only be chosen as a target by one attacking group.
                var target = allGroupsWithUnits
                    .Where(g => !alreadyPicked.Contains(g))
                    .Where(g => g.affiliation != group.affiliation)
                    .Where(g => group.DamageTo(g) > 0)
                    .OrderByDescending(g => group.DamageTo(g))
                    .ThenByDescending(g => g.EffectivePower)
                    .ThenByDescending(g => g.unitType.initiative)
                    .FirstOrDefault();

                alreadyPicked.Add(target);
                targets.Add(group, target);
            }

            return targets;
        }

        public int DoAttack(Dictionary<Group, Group> targets)
        {
            // During the attacking phase, each group deals damage to the target it selected, if any. Groups attack in 
            // decreasing order of initiative, regardless of whether they are part of the infection or the immune system. 
            // (If a group contains no units, it cannot attack.)
            var allAttackers = immuneSystemArmy
             .Union(infectionArmy)
             .Where(g => g.numUnits > 0)
             .OrderByDescending(g => g.unitType.initiative)
             .ToList();

            int numKilledThisRound = 0;

            foreach(var group in allAttackers)
            {
                var target = targets[group];
                if (target != null)
                {
                    numKilledThisRound += target.TakeDamageFrom(group);
                }
            }

            return numKilledThisRound;
        }
    }
}
