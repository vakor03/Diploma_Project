using System;
using System.Collections.Generic;

namespace _Project.Infrastructure.MVP.Core {
    public class ActiveWindowDataHolder {
        public Dictionary<Type, WindowStatus> WindowStatusMap = new Dictionary<Type, WindowStatus>();
        public Dictionary<Type, WindowBehaviour> WindowGameObjectMap = new Dictionary<Type, WindowBehaviour>();

        public WindowStatus GetWindowStatus<TWindow>() where TWindow : WindowBehaviour =>
            WindowStatusMap.GetValueOrDefault(typeof(TWindow), WindowStatus.Closed);
    }
}