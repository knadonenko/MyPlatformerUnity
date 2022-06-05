using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Model.Definitions.Editor
{
    [CustomPropertyDrawer(typeof(InventoryIdAttribute))]
    public class InventoryAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var defs = DefsFacade.InstDef.Items.ItemsForEditor;
            var ids = defs.Select(itemDef => itemDef.Id).ToList();
            var index = Mathf.Max(ids.IndexOf(property.stringValue), 0);

            index = EditorGUI.Popup(position, property.displayName, index, ids.ToArray());
            property.stringValue = ids[index];
        }
    }
}