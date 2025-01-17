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

using MB;

namespace Game
{
	public class WeaponAim : Weapon.Module
	{
        public bool IsOn { get; protected set; }

        public float Target => IsOn ? 1f : 0f;

        public float Rate { get; protected set; }
        public delegate void RateChangeDelegate(float rate);
        public event RateChangeDelegate OnRateChange;

        public Modifier.Constraint Constraint { get; protected set; }

        public bool CanPerform
        {
            get
            {
                if (Constraint.Value) return false;

                return true;
            }
        }

        [field: SerializeField, DebugOnly]
        public WeaponAimSpeed Speed { get; protected set; }

        [field: SerializeField, DebugOnly]
        public Modules<WeaponAim> Modules { get; protected set; }
        public abstract class Module : Weapon.Behaviour, IModule<WeaponAim>
        {
            [field: SerializeField, DebugOnly]
            public WeaponAim Aim { get; protected set; }

            public Weapon Weapon => Aim.Weapon;

            public virtual void Set(WeaponAim value) => Aim = value;
        }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor : Weapon.IProcessor
        {
            bool Input { get; }

            float Rate { set; }

            void ClearInput();
        }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponAim>(this);
            Modules.Register(Weapon.Behaviours, SegmentScope.Global);

            Speed = Modules.Depend<WeaponAimSpeed>();

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Processor = Weapon.GetProcessor<IProcessor>(this);

            Constraint = new Modifier.Constraint();
        }

        public override void Initialize()
        {
            base.Initialize();

            Weapon.OnProcess += Process;

            Weapon.Activation.OnDisable += DisableCallback;
        }

        void DisableCallback()
        {
            Rate = 0f;
        }

        void Process()
        {
            if (Processor.Input)
            {
                if (CanPerform)
                    IsOn = Processor.Input;
                else
                    IsOn = false;
            }
            else
            {
                IsOn = false;
            }

            if (Rate != Target)
            {
                Rate = Mathf.MoveTowards(Rate, Target, Speed.Value * Time.deltaTime);
                Processor.Rate = Rate;
                OnRateChange?.Invoke(Rate);
            }
        }

        public virtual void ClearInput() => Processor.ClearInput();
    }
}