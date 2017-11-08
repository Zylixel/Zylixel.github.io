#region

using wServer.realm;
using wServer.realm.entities;

#endregion

namespace wServer.logic.transitions
{
    public class HpLessTransition : Transition
    {
        //State storage: none

        private readonly double _threshold;

        public HpLessTransition(double threshold, string targetState)
            : base(targetState)
        {
            _threshold = threshold;
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            if (_threshold > 1.0)
                return (host as Enemy).HP < _threshold;
            return ((host as Enemy).HP/host.ObjectDesc.MaxHp) < _threshold;
        }
    }
}