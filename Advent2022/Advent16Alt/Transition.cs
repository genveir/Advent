using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Advent16Alt
{
    public class Transition
    {
        public State NewState;
        public long TransitionValue;

        public Transition(State newState, long transitionValue)
        {
            NewState = newState;
            TransitionValue = transitionValue;
        }

        public override string ToString() =>
           $"{TransitionValue} -> {NewState}";

        public override int GetHashCode()
        {
            return NewState.StateString.GetHashCode() + (int)TransitionValue;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Transition;
            return other.TransitionValue == TransitionValue &&
                other.NewState.StateString == NewState.StateString;
        }
    }
}
