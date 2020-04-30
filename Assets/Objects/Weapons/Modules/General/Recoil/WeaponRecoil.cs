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

namespace Game
{
    public abstract class WeaponRecoil : Weapon.Module, Weapon.IEffect
    {
        [SerializeField]
        protected float scale = 1f;
        public float Scale
        {
            get => scale;
            set => scale = value;
        }

        [SerializeField]
        protected Transform context;
        public Transform Context
        {
            get => context;
            set => context = value;
        }

        public ValueRange kick;

        [SerializeField]
        protected SwayData sway;
        public SwayData Sway { get { return sway; } }

        [SerializeField]
        protected SpeedData speed;
        public SpeedData Speed { get { return speed; } }

        public Vector3 Target { get; protected set; }

        public Vector3 Value { get; protected set; }

        [Serializable]
        public struct ValueRange
        {
            [SerializeField]
            float min;
            public float Min { get { return min; } }

            [SerializeField]
            float max;
            public float Max { get { return max; } }

            public float Random => UnityEngine.Random.Range(min, max);

            public float Lerp(float t) => Mathf.Lerp(min, max, t);

            public ValueRange(float min, float max)
            {
                this.min = min;
                this.max = max;
            }
        }

        [Serializable]
        public struct SwayData
        {
            [SerializeField]
            float vertical;
            public float Vertical { get { return vertical; } }

            [SerializeField]
            float horizontal;
            public float Horizontal { get { return horizontal; } }

            public SwayData(float vertical, float horizontal)
            {
                this.vertical = vertical;
                this.horizontal = horizontal;
            }
            public SwayData(float value) : this(value, value)
            {

            }
        }

        [Serializable]
        public struct SpeedData
        {
            [SerializeField]
            float set;
            public float Set { get { return set; } }

            [SerializeField]
            float reset;
            public float Reset { get { return reset; } }

            public SpeedData(float set, float reset)
            {
                this.set = set;
                this.reset = reset;
            }
        }
        
        protected virtual void Reset()
        {
            context = transform;
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnAction += Action;

            Weapon.OnLateProcess += LateProcess;
        }

        void Action()
        {
            if(enabled) Target = CalculateTarget() * scale;
        }

        protected abstract Vector3 CalculateTarget();

        void LateProcess(Weapon.IProcessData data)
        {
            Apply(-Value);
            {
                Value = Vector3.Lerp(Value, Target, speed.Set * Time.deltaTime);
                Target = Vector3.Lerp(Target, Vector3.zero, speed.Reset * Time.deltaTime);
            }
            Apply(Value);
        }

        protected abstract void Apply(Vector3 value);
    }
}