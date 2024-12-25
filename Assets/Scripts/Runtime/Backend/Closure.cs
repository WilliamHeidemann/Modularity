using System;

namespace Runtime.Backend
{
    public readonly struct Closure<TContext>
    {
        private readonly Delegate _delegate;
        private readonly TContext _context;

        private Closure(Delegate @delegate, TContext context = default)
        {
            _delegate = @delegate;
            _context = context;
        }

        public static Closure<TContext> Create(Action action) => new(action);
        public static Closure<TContext> Create(Action<TContext> action, TContext context) => new(action, context);
        public static Closure<TContext> Create<TResult>(Func<TResult> func) => new(func);
        public static Closure<TContext> Create<TResult>(Func<TContext, TResult> func, TContext context) => new(func, context);
        // public static Closure<TContext> Create(Predicate<TContext> predicate) => new(predicate);
        // public static Closure<TContext> Create(Predicate<TContext> predicate, TContext context) => new(predicate, context);

        public void Invoke()
        {
            switch (_delegate)
            {
                case Action action:
                    action();
                    break;
                case Action<TContext> actionWithContext:
                    actionWithContext(_context);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public TResult Invoke<TResult>()
        {
            return _delegate switch
            {
                Func<TResult> func => func(),
                Func<TContext, TResult> funcWithContext => funcWithContext(_context),
                _ => throw new ArgumentException()
            };
        }
        
        public bool InvokePredicate()
        {
            if (_delegate is Predicate<TContext> predicate)
            {
                return predicate(_context);
            }

            throw new ArgumentException();
        }
    }
}