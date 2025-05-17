using System.Collections.Generic;

namespace _Project.Features.ExperienceModule.Orbs {
    public interface IEnumValuesProvider {
        public List<T> GetEnumValues<T>() where T : System.Enum;
    }
}