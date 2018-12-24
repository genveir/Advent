using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.Advent24
{
    class Fight
    {
        public List<Group> immuneSystem;
        public List<Group> infection;

        public Fight(List<Group> immuneSystem, List<Group> infection)
        {
            this.immuneSystem = immuneSystem;
            this.infection = infection;
        }

        public void Reset()
        {
            foreach (var group in immuneSystem) group.Reset();
            foreach (var group in infection) group.Reset();
        }

        public void Boost(int boost)
        {
            foreach (var group in immuneSystem) group.Boost(boost);
        }

        public Affiliation Winner;
        public bool DoRound(bool print)
        {
            if (print)
            {
                Console.WriteLine("Immune System:");
                for (int n = 0; n < immuneSystem.Count; n++)
                    Console.WriteLine(string.Format("Group {0} contains {1} units", immuneSystem[n].unitType.initiative, immuneSystem[n].numUnits));

                Console.WriteLine("Infection:");
                for (int n = 0; n < infection.Count; n++)
                    Console.WriteLine(string.Format("Group {0} contains {1} units", infection[n].unitType.initiative, infection[n].numUnits));

                Console.WriteLine();
            }

            var targets = DoTargetSelection(print);
            int numKilledThisRound = DoAttack(targets, print);

            if (print)
            {
                Console.WriteLine("Round done");
                Console.ReadLine();
            }

            if (numKilledThisRound == 0 || immuneSystem.Sum(g => g.numUnits) == 0) { Winner = Affiliation.Infection; return false; }
            if (infection.Sum(g => g.numUnits) == 0) { Winner = Affiliation.ImmuneSystem; return false; }

            return true;
        }

        public Dictionary<Group, Group> DoTargetSelection(bool print)
        {
            // During the target selection phase, each group attempts to choose one target.In decreasing order of effective power, 
            //groups choose their targets; in a tie, the group with the higher initiative chooses first.
            var allGroups = immuneSystem
                 .Union(infection)
                 .Where(g => g.numUnits > 0)
                 .OrderByDescending(g => g.EffectivePower)
                 .ThenByDescending(g => g.unitType.initiative)
                 .ToList();

            var targets = new Dictionary<Group, Group>();
            var alreadyPicked = new HashSet<Group>();
            foreach (var group in allGroups)
            {
                // The attacking group chooses to target the group in the enemy army to which it would deal the most damage
                // (after accounting for weaknesses and immunities, but not accounting for whether the defending group has 
                // enough units to actually receive all of that damage).

                // If an attacking group is considering two defending groups to which it would deal equal damage, 
                // it chooses to target the defending group with the largest effective power; if there is still a tie, it 
                // chooses the defending group with the highest initiative.If it cannot deal any defending groups damage, it
                // does not choose a target.Defending groups can only be chosen as a target by one attacking group.
                var target = allGroups
                    .Where(g => !alreadyPicked.Contains(g))
                    .Where(g => g.affiliation != group.affiliation)
                    .Where(g => group.DamageTo(g) > 0)
                    .OrderByDescending(g => group.DamageTo(g))
                    .ThenByDescending(g => g.EffectivePower)
                    .ThenByDescending(g => g.unitType.initiative)
                    .FirstOrDefault();

                if (print)
                {
                    Console.WriteLine(
                        string.Format("{0} group {1} will attack {2}",
                            group.affiliation, group.unitType.initiative, 
                            (target == null) ? "noone" : string.Format("group {0} and deal {1} damage", 
                                target.unitType.initiative, group.DamageTo(target))));
                }

                alreadyPicked.Add(target);
                targets.Add(group, target);
            }

            if (print) Console.WriteLine();
            return targets;
        }

        public int DoAttack(Dictionary<Group, Group> targets, bool print)
        {
            // During the attacking phase, each group deals damage to the target it selected, if any. Groups attack in 
            // decreasing order of initiative, regardless of whether they are part of the infection or the immune system. 
            // (If a group contains no units, it cannot attack.)
            var allAttackers = immuneSystem
             .Union(infection)
             .Where(g => g.numUnits > 0)
             .OrderByDescending(g => g.unitType.initiative)
             .ToList();

            int numKilledThisRound = 0;

            foreach(var group in allAttackers)
            {
                var target = targets[group];
                if (target != null)
                {
                    if (print)
                    {
                        var numKilled = Math.Min(target.numUnits, group.DamageTo(target) / target.unitType.hitpoints);
                        Console.WriteLine(
                            string.Format("{0} group {1} attacks group {2} for {3} damage, killing {4} units leaving {5}",
                                group.affiliation, 
                                group.unitType.initiative, 
                                target.unitType.initiative, 
                                group.DamageTo(target), 
                                numKilled,
                                target.numUnits - numKilled));
                    }

                    numKilledThisRound += target.TakeDamageFrom(group);
                }
            }

            return numKilledThisRound;
        }
    }
}
