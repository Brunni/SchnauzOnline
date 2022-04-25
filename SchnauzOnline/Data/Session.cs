using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchnauzOnline.Data
{
    public class Session
    {
        public int SessionNummer { get; }

        private IList<SessionMember> Members { get; } = new List<SessionMember>();

        private IList<ChatMessage> ChatMessages { get; } = new List<ChatMessage>();
    }
}
