using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniExtension
{
    [CustomPropertyDrawer(typeof(IntRange))]
    public class IntRangeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.flexShrink = 0;
            root.style.marginLeft = 3;

            var label = new Label(property.displayName);
            label.style.flexShrink = 0;
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.minWidth = 120;
            root.Add(label);

            var fieldsContainer = new VisualElement();
            fieldsContainer.style.flexDirection = FlexDirection.Row;
            fieldsContainer.style.flexGrow = 1;
            fieldsContainer.style.flexShrink = 0;
            fieldsContainer.style.justifyContent = Justify.FlexEnd;
            root.Add(fieldsContainer);

            var minProperty = property.FindPropertyRelative("_min");
            var maxProperty = property.FindPropertyRelative("_max");

            var minField = new IntegerField("min");
            minField.labelElement.style.flexBasis = 30;
            minField.labelElement.style.minWidth = 30;
            minField.BindProperty(minProperty);

            var maxField = new IntegerField("max");
            maxField.labelElement.style.flexBasis = 30;
            maxField.labelElement.style.minWidth = 30;
            maxField.BindProperty(maxProperty);

            minField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue > maxField.value)
                    maxField.value = evt.newValue;
            });

            maxField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue < minField.value)
                    minField.value = evt.newValue;
            });

            fieldsContainer.Add(minField);
            fieldsContainer.Add(maxField);

            return root;
        }
    }
}
