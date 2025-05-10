using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Infrastructure.MVP.Core {
    public abstract class WindowBehaviour : IDisposable {
        private readonly List<PresenterBehaviour> _presenters = new List<PresenterBehaviour>();
        
        public GameObject GameObject { get; set; }

        public IEnumerable<PresenterBehaviour> GetAllWindowPresenters() =>
            _presenters;

        public void RegisterPresenter(PresenterBehaviour presenter) =>
            _presenters.Add(presenter);

        public void UnregisterPresenter(PresenterBehaviour presenter) =>
            _presenters.Add(presenter);

        public void Dispose() {
            foreach (PresenterBehaviour presenterBehaviour in _presenters)
                presenterBehaviour.OnDisposed();
            
            _presenters.Clear();
        }
    }
}