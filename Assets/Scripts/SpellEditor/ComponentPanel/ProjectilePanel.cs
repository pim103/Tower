using SpellEditor.PanelUtils;
using UnityEngine;

namespace SpellEditor.ComponentPanel
{
    public class ProjectilePanel : MonoBehaviour
    {
        [SerializeField] private DropdownMultiSelector dropdownMultiSelector;

        public void InitProjectilePanel()
        {
            dropdownMultiSelector.InitDropdownMultiSelect();
        }
    }
}
