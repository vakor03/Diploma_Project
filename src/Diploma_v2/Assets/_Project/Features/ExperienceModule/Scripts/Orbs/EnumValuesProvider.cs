using System.Collections.Generic;
using System.Linq;

namespace _Project.Features.ExperienceModule {
    public class EnumValuesProvider : IEnumValuesProvider {
        public List<T> GetEnumValues<T>() where T : System.Enum =>
            System.Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }
}