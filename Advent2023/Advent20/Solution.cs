using System;
using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;
using Advent2023.Shared.InputParsing;

namespace Advent2023.Advent20;

public class Solution : ISolution
{
    public string[] lines;

    public Dictionary<string, Module> Modules { get; set; }

    public Solution(string input)
    {
        lines = Input.GetInputLines(input).ToArray();
    }
    public Solution() : this("Input.txt") { }

    public void Reset()
    {
        var inputParser = new InputParser<Module>("line");

        Modules = inputParser.Parse(lines).ToDictionary(m => m.Name, m => m);

        var allTargets = Modules.Values.SelectMany(m => m.TargetNames).Distinct();
        var outputs = allTargets.Where(tn => !Modules.ContainsKey(tn)).ToArray();
        foreach (var output in outputs)
            Modules.Add(output, new UntypedModule(output, Array.Empty<string>()));

        foreach (var module in Modules.Values)
        {
            module.SetTargets(Modules);
        }

        foreach (var module in Modules.Values)
        {
            module.AfterAllTargetsAreSet();
        }
    }

    public class Pulse
    {
        public Module Source { get; }
        public Module Target { get; }
        public bool IsHigh { get; }

        public Pulse(Module source, Module target, bool isHigh)
        {
            Source = source;
            Target = target;
            IsHigh = isHigh;
        }

        private const string Low = " -low-> ";
        private const string High = " -high-> ";

        public override string ToString() => $"{Source?.Name ?? "button"}{(IsHigh ? High : Low)}{Target.Name}";
    }

    public abstract class Module
    {
        public string Name { get; set; }
        public string[] TargetNames { get; set; }
        public List<Module> Sources { get; set; } = new();
        public Module[] Targets { get; set; }

        public bool Value { get; protected set; }
        public bool? LastSent { get; protected set; }

        [ComplexParserTarget("name -> targets")]
        public static Module CreateModule(string name, string[] targetNames)
        {
            return name[0] switch
            {
                '&' => new ConjunctionModule(name.Substring(1), targetNames),
                '%' => new FlipFlopModule(name.Substring(1), targetNames),
                'b' => new BroadcastModule(name, targetNames),
                _ => new UntypedModule(name, targetNames)
            };
        }

        public Module(string name, string[] targetNames)
        {
            Name = name;
            TargetNames = targetNames.Select(tn => tn.Trim()).ToArray();
        }

        public void SetTargets(Dictionary<string, Module> modules)
        {
            Targets = TargetNames.Select(tn => modules[tn]).ToArray();

            foreach (var module in Targets) module.Sources.Add(this);
        }

        public virtual void AfterAllTargetsAreSet() { }
        public virtual void OnRoundStart() { }

        public abstract void Receive(Module source, bool isHigh);
        public virtual IEnumerable<Pulse> Send()
        {
            LastSent = Value;

            return Targets.Select(t => new Pulse(this, t, Value));
        }

        public IEnumerable<Pulse> HandlePulse(Module source, bool isHigh)
        {
            Receive(source, isHigh);

            return Send();
        }

        public abstract char TypePrefix { get; }
        public override string ToString() => $"{TypePrefix}{Name} {Value}";
    }

    public class BroadcastModule : Module
    {
        public BroadcastModule(string name, string[] targets) : base(name, targets) { }

        public override void Receive(Module source, bool isHigh)
        {
            Value = isHigh;
        }

        public override char TypePrefix => '.';
    }

    public class FlipFlopModule : Module
    {
        public FlipFlopModule(string name, string[] targetNames) : base(name, targetNames) { }

        private bool LastReceived = false;

        public override void Receive(Module source, bool isHigh)
        {
            LastReceived = isHigh;
            if (!isHigh) Value = !Value;
        }

        public override IEnumerable<Pulse> Send()
        {
            LastSent = null;

            if (!LastReceived) return base.Send();
            else return new List<Pulse>();
        }

        public override char TypePrefix => '%';
    }

    public class ConjunctionModule : Module
    {
        public ConjunctionModule(string name, string[] targetNames) : base(name, targetNames) { }

        private Dictionary<string, bool> LastValues = null;

        public bool HasFired = false;

        public override void OnRoundStart() => HasFired = false;

        public override void AfterAllTargetsAreSet()
        {
            LastValues = Sources.ToDictionary(s => s.Name, _ => false);
        }

        public override void Receive(Module source, bool isHigh)
        {
            LastValues[source.Name] = isHigh;

            Value = !LastValues.Values.All(b => b);

            if (Value == false) HasFired = true;
        }

        public override char TypePrefix => '&';
    }

    public class UntypedModule : Module
    {
        public UntypedModule(string name, string[] targetNames) : base(name, targetNames) { }

        public override char TypePrefix => '_';

        public override void Receive(Module source, bool isHigh) { }
    }

    public (long lowCount, long highCount) PushButton()
    {
        foreach (var module in Modules.Values) module.OnRoundStart();

        var broadcastModule = Modules["broadcaster"];
        var pulseQueue = new Queue<Pulse>();

        pulseQueue.Enqueue(new Pulse(null, broadcastModule, false));

        long lowCount = 0;
        long highCount = 0;
        while (pulseQueue.Count > 0)
        {
            var pulse = pulseQueue.Dequeue();

            if (pulse.IsHigh) highCount++;
            else lowCount++;

            var newPulses = pulse.Target.HandlePulse(pulse.Source, pulse.IsHigh);

            foreach (var newPulse in newPulses)
                pulseQueue.Enqueue(newPulse);
        }

        return (lowCount, highCount);
    }

    public object GetResult1()
    {
        Reset();

        long low = 0;
        long high = 0;
        for (int n = 0; n < 1000; n++)
        {
            var (lowCount, highCount) = PushButton();
            low += lowCount;
            high += highCount;
        }

        return low * high;
    }

    public ConjunctionModule[] IdentifyGroupAccumulators()
    {
        var broadcastTargets = Modules["broadcaster"].Targets;

        var result = new ConjunctionModule[broadcastTargets.Length];
        for (int n = 0; n < broadcastTargets.Length; n++)
        {
            result[n] = IdentifyGroupAccumulator(broadcastTargets[n]);
        }
        return result;
    }

    public ConjunctionModule IdentifyGroupAccumulator(Module module)
    {
        if (module.Targets.Length == 1)
            return IdentifyGroupAccumulator(module.Targets[0]);
        else return module.Targets.Single(t => t is ConjunctionModule) as ConjunctionModule;
    }

    public object GetResult2()
    {
        // BC points to four groups, each group has an accumulator. Id and figure out periodicity

        Reset();

        var accumulators = IdentifyGroupAccumulators();

        var accumulatorFires = new List<long>[accumulators.Length];
        for (int n = 0; n < accumulatorFires.Length; n++) accumulatorFires[n] = new();

        int numSet = 0;
        long round = 0;
        while (numSet != accumulators.Length * 10)
        {
            round++;
            PushButton();

            for (int n = 0; n < accumulators.Length; n++)
            {
                if (accumulators[n].HasFired && accumulatorFires[n].Count < 10)
                {
                    accumulatorFires[n].Add(round);
                    numSet++;
                }
            }
        }
        var accumulatorStarts = accumulatorFires.Select(af => af[0]).ToArray();
        var accumulatorPeriods = accumulatorFires.Select(af => af[1] - af[0]).ToArray();

        // volledig periodiek vanaf de start

        return accumulatorStarts.Aggregate((a, b) => a * b);
    }
}
