using System.Linq;
using wServer.realm;

namespace wServer.logic.transitions
{
    public class ChatTransition : Transition
    {
        private readonly string[] _texts;
        private bool _transit;

        public ChatTransition(string targetState, params string[] texts)
            : base(targetState)
        {
            _texts = texts ?? Empty<string>.Array;
            _transit = false;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            _transit = false;
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            return _transit;
        }

        public void OnChatReceived(string text)
        {
            if (_texts.Contains(text))
                _transit = true;
        }
    }
}
