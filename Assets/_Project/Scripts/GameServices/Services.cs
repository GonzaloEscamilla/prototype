using System;

namespace _Project.Scripts.GameServices
{
    public static class Services
    {
        private static ServiceLocator Instance => _instance ?? (_instance = new ServiceLocator());

        private static ServiceLocator _instance;

        public static void Add<T>(T service) where T : class
        {
            Services.Instance.AddService<T>(service);
        }

        public static void WaitFor<T>(Action<T> onServiceAdded) where T : class
        {
            Services.Instance.WaitFor<T>(onServiceAdded);
        }

        public static void Clear()
        {
            Services.Instance.Clear();
        }

        public static bool Exists<T>() where T : class
        {
            return Services.Instance.Contains<T>();
        }

        public static T Get<T>() where T : class
        {
            return Services.Instance.GetService<T>();
        }
    }
}