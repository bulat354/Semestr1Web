using System.Runtime.Caching;

namespace MyServer.Managers
{
    public class SessionManager
    {
        public static SessionManager Instance { get; } = new SessionManager();

        private MemoryCache _sessions;
        private CacheItemPolicy DefaultPolicy
        {
            get
            {
                var policy = new CacheItemPolicy();
                policy.SlidingExpiration = TimeSpan.FromMinutes(2);
                return policy;
            }
        }

        private SessionManager()
        {
            _sessions = MemoryCache.Default;
        }

        public Session CreateSession(int accountId, CacheItemPolicy policy = null)
        {
            if (policy == null)
                policy = DefaultPolicy;

            var guid = Guid.NewGuid().ToString();
            var session = new Session(guid, accountId);

            while (_sessions.Contains(guid))
            {
                var tmp = (Session)_sessions.Get(guid);
                if (tmp.AccountId == accountId)
                {
                    _sessions.Set(guid, session, policy);
                    return session;
                }
                guid = Guid.NewGuid().ToString();
            }

            _sessions.Add(guid, session, policy);
            return session;
        }

        public bool CheckSession(string guid)
        {
            return _sessions.Contains(guid);
        }

        public Session GetSession(string guid)
        {
            if (CheckSession(guid))
                return (Session)_sessions.Get(guid);

            throw new ArgumentException("Session with the same guid doesn't exist");
        }

        public static CacheItemPolicy GetAbsolutePolicy(int minutes)
        {
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(minutes);
            return policy;
        }

        public static CacheItemPolicy GetSlidingPolicy(int minutes)
        {
            var policy = new CacheItemPolicy();
            policy.SlidingExpiration = TimeSpan.FromMinutes(minutes);
            return policy;
        }

        public static CacheItemPolicy GetSessionPolicy()
        {
            var policy = new CacheItemPolicy();
            return policy;
        }
    }

    public class Session
    {
        public string Id { get; }
        public int AccountId { get; }

        public Session(string id, int accountId)
        {
            Id = id;
            AccountId = accountId;
        }
    }
}
