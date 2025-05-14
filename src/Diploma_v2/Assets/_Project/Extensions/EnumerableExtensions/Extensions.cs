using System;

namespace Global.Helpers.Scripts {
	public static class Extensions {
		public static T With<T>(this T obj, Action action) {
			action?.Invoke();
			return obj;
		}

		public static T With<T>(this T obj, Action<T> action) {
			action?.Invoke(obj);
			return obj;
		}
	}
}