using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions
{
    public abstract class Expression
    {
        public virtual bool IsSet => false;
        public bool Mutable = true;

        private long? _value;
        public long? Value 
        {
            get => _value;
            set
            {
                if (!Mutable) throw new ImmutableObjectException("can't set Value on immutable expressions");
                _value = value;
            }
        }

        private Expression _left, _right;
        public Expression Left 
        {
            get => _left;
            set
            {
                if (!Mutable) throw new ImmutableObjectException("can't set Left on immutable expressions");
                _left = value;
            }
        }
        public Expression Right 
        {
            get => _right;
            set
            {
                if (!Mutable) throw new ImmutableObjectException("can't set Right on immutable expressions");
                _right = value;
            }
        }

        public static long _idCursor = 0;
        public long Id { get; }

        public Expression(Expression left, Expression right, long? value = null, bool mutable = false) 
        { 
            Id = _idCursor++;

            Left = left;
            Right = right;
            Value = value;
            Mutable = mutable;
        }

        private long _uniqueCount;
        public virtual long UniqueSimplifyableExpressionCount()
        {
            var val = 1 - _uniqueCount;
            _uniqueCount = 1;
            return val + (Left?.UniqueSimplifyableExpressionCount() ?? 0) + (Right?.UniqueSimplifyableExpressionCount() ?? 0);
        }

        private long _count;
        public long Count()
        {
            if (_count == 0 || Mutable)
            {
                _count = (Left?.Count() ?? 0) + 1 + (Right?.Count() ?? 0);
            }
            return _count;
        }

        private long _depth;
        public long Depth()
        {
            if (_depth == 0 || Mutable)
            {
                _depth = Math.Max((Left?.Depth() ?? 0), (Right?.Depth() ?? 0)) + 1;
            }
            return _depth;
        }

        public abstract Expression Simplify();

        public abstract bool IsEquivalentTo(Expression other);

        public abstract string PrintToDepth(int depth);
    }
}
