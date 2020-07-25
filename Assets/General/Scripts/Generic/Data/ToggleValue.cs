﻿using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine.UIElements;

namespace Game
{
    [Serializable]
    public abstract class ToggleValue
    {
        [SerializeField]
        protected bool enabled;
        public bool Enabled { get { return enabled; } }

#if UNITY_EDITOR
        public abstract class BaseInspector : PropertyDrawer
        {
            protected SerializedProperty property;
            protected SerializedProperty enabled;
            protected SerializedProperty value;

            public const float ToggleSize = 15f;
            public const float ToggleSpacing = 2f;

            protected virtual void Init(SerializedProperty property)
            {
                this.property = property;

                enabled = property.FindPropertyRelative("enabled");
                value = property.FindPropertyRelative("value");
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                Init(property);

                if (enabled.boolValue && value.isExpanded)
                    return EditorGUI.GetPropertyHeight(value, label, true);

                return EditorGUIUtility.singleLineHeight;
            }

            public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
            {
                rect = EditorGUI.IndentedRect(rect);

                var indentLevel = EditorGUI.indentLevel;

                EditorGUI.indentLevel = 0;
                {
                    Init(property);

                    DrawToggle(ref rect, property, label);

                    if (enabled.boolValue)
                        DrawValue(ref rect, property, label);
                    else
                        DrawLabel(ref rect, property, label);
                }
                EditorGUI.indentLevel = indentLevel;
            }

            protected virtual void DrawToggle(ref Rect rect, SerializedProperty property, GUIContent label)
            {
                enabled.boolValue = EditorGUI.Toggle(new Rect(rect.x, rect.y, ToggleSize, ToggleSize), enabled.boolValue);
                
                rect.x += ToggleSize + ToggleSpacing;
                rect.width -= ToggleSize + ToggleSpacing;
            }
            protected virtual void DrawLabel(ref Rect rect, SerializedProperty property, GUIContent label)
            {
                var width = GUI.skin.label.CalcSize(label).x + 10f;

                EditorGUI.LabelField(new Rect(rect.x, rect.y, width, rect.height), label);

                rect.x += width;
                rect.width -= width;
            }
            protected virtual void DrawValue(ref Rect rect, SerializedProperty property, GUIContent label)
            {
                var value = property.FindPropertyRelative("value");

                var labelWidth = GUI.skin.label.CalcSize(label).x;

                EditorGUIUtility.labelWidth -= ToggleSize + ToggleSpacing;

                if (HasExpandControl(value))
                {
                    var offset = 10f;

                    rect.x += offset;
                    rect.width -= offset;
                }

                EditorGUI.PropertyField(rect, value, label, true);

                EditorGUIUtility.labelWidth += ToggleSize + ToggleSpacing;
            }

            protected virtual bool HasExpandControl(SerializedProperty target)
            {
                if (target.hasVisibleChildren == false) return false;

                if (target.type.Contains("Vector")) return false;

                return true;
            }
        }

        [CustomPropertyDrawer(typeof(ToggleValue), true)]
        public class DefaultInspector : BaseInspector
        {

        }
#endif
    }

    [Serializable]
    public abstract class ToggleValue<TValue> : ToggleValue
    {
        [SerializeField]
        protected TValue value;
        public TValue Value { get { return value; } }

        public virtual TValue Evaluate(TValue defaultValue)
        {
            if (enabled) return value;

            return defaultValue;
        }

        public ToggleValue() : this(false, default(TValue))
        {

        }
        public ToggleValue(bool enabled, TValue value)
        {
            this.enabled = enabled;

            this.value = value;
        }
    }

    #region Defaults
    [Serializable]
    public class IntToggleValue : ToggleValue<int> { }

    [Serializable]
    public class FloatToggleValue : ToggleValue<float> { }

    [Serializable]
    public class BoolToggleValue : ToggleValue<bool> { }

    [Serializable]
    public class StringToggleValue : ToggleValue<string> { }

    [Serializable]
    public class ColorToggleValue : ToggleValue<Color>
    {
        public ColorToggleValue()
        {
            value = Color.white;
        }
    }

    [Serializable]
    public class Vector2ToggleValue : ToggleValue<Vector2> { }

    [Serializable]
    public class Vector3ToggleValue : ToggleValue<Vector3> { }

    [Serializable]
    public class AnimationCurveToggleValue : ToggleValue<AnimationCurve>
    {
        public virtual float Evaluate(float time)
        {
            if (enabled)
                return value.Evaluate(time);

            return time;
        }
    }
    #endregion
}