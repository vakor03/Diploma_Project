using TMPro;
using UnityEngine;

namespace _Project.Features.UIModule.MetaDataUI {
    internal class MetaDataView : MetaDataViewBase {
        [SerializeField] private TMP_Text _version;
    
        public override void SetCurrentVersion(string version) =>
            _version.text = version;
    }
}