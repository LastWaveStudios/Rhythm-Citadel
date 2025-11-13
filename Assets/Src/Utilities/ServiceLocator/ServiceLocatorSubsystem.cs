using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Utilities.ServiceLocator
{
    /// <summary>
    /// It will be a serviceLocator that take the services from the scene instead of generate them, when the scene changes,
    /// so is not the standard implementation
    /// </summary>
    public class ServiceLocatorSubsystem : Subsystem<ServiceLocatorSubsystem>
    {
        [SerializeField] private List<AService> _services;

        public bool IsInitialize { get; private set; } = false;
        private Action onInitialize = delegate { };

        private void Start()
        {
            foreach (AService service in _services)
            {
                service.Init();
            }
            IsInitialize = true;
            onInitialize.Invoke();
            onInitialize = null;
        }

        /// <summary>
        /// Get the service of type T,
        /// Important note do not use if the IsInitialize is not true, have an event and a property to handle it
        /// </summary>
        /// <typeparam name="T">Type of the service that want to get</typeparam>
        /// <returns>Return the first service of type T or null if not found that service</returns>
        public T GetService<T>() where T : AService
        {
            return _services.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Use this method to take a callback when the serviceLocator is Initialize and if is already init the callback is immediate
        /// </summary>
        /// <param name="callbackWhenReady">callback do when is ready</param>
        public static void SubscribeToInitialice(Action callbackWhenReady)
        {
            if (Instance.IsInitialize)
            {
                callbackWhenReady();
                return;
            }
            else
            {
                Instance.onInitialize += callbackWhenReady;
            }
        }
    }
}