#region

using wServer.realm;

#endregion

namespace wServer.logic.transitions
{
    public class TimedTransition : Transition
    {
        //State storage: cooldown timer

        private readonly bool _randomized;
        private readonly int _time;

        public TimedTransition(int time, string targetState, bool randomized = false)
            : base(targetState)
        {
            _time = time;
            _randomized = randomized;
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            int cool;
            if (state == null) cool = _randomized ? Random.Next(_time) : _time;
            else cool = (int) state;

            bool ret = false;
            if (cool <= 0)
            {
                ret = true;
                cool = _time;
            }
            else
                cool -= time.thisTickTimes;

            state = cool;
            return ret;
        }
    }
}