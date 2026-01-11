using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UniExtension
{
    [CustomPropertyDrawer(typeof(IntRange))]
    public class IntRangeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Styles/RootStyle.uss");
            var root = new VisualElement();
            root.styleSheets.Add(styleSheet);
            root.AddToClassList("align-horizontal");
            root.AddToClassList("flex-grow");

            var label = new Label(property.displayName);
            label.style.marginLeft = 4;
            label.AddToClassList("flex-grow");
            root.Add(label);

            var fieldsContainer = new VisualElement();
            fieldsContainer.AddToClassList("align-horizontal");
            root.Add(fieldsContainer);

            var minProperty = property.FindPropertyRelative("_min");
            var maxProperty = property.FindPropertyRelative("_max");

            var minField = new IntegerField("min");
            minField.BindProperty(minProperty);

            var maxField = new IntegerField("max");
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
