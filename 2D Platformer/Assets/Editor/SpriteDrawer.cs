using UnityEngine;
using UnityEditor;
using Platformer.Extensions;

namespace Platformer.Editor
{
    [CustomPropertyDrawer(typeof(Sprite))]
    public class SpriteDrawer : PropertyDrawer
    {

        private static GUIStyle s_TempStyle = new GUIStyle();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var ident = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect spriteRect;

            //create object field for the sprite
            spriteRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.objectReferenceValue = EditorGUI.ObjectField(spriteRect, property.name.FirstCharToUpper(), property.objectReferenceValue, typeof(Sprite), false);

            //if this is not a repain or the property is null exit now
            if (Event.current.type != EventType.Repaint || property.objectReferenceValue == null)
                return;

            //draw a sprite
            var sp = property.objectReferenceValue as Sprite;

            spriteRect.y += EditorGUIUtility.singleLineHeight + 4;
            spriteRect.width = 64;
            spriteRect.height = 64;
            s_TempStyle.normal.background = sp.texture;
            s_TempStyle.Draw(spriteRect, GUIContent.none, false, false, false, false);

            EditorGUI.indentLevel = ident;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + 70f;
        }
    }
}

