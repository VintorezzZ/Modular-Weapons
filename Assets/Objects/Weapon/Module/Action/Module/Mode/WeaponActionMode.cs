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
    public class WeaponActionMode : Weapon.Module
    {
        public IList<IState> List { get; protected set; }
        public interface IState
        {
            bool enabled { get; set; }
        }

        public virtual void Set(int index)
        {
            for (int i = 0; i < List.Count; i++)
                List[i].enabled = i == index;
        }

        public int Index { get; protected set; }

        [field: SerializeField, DebugOnly]
        public Modules<WeaponActionMode> Modules { get; protected set; }
        public abstract class Module : Weapon.Behaviour, IModule<WeaponActionMode>
        {
            [field: SerializeField, DebugOnly]
            public WeaponActionMode Mode { get; protected set; }

            public Weapon Weapon => Mode.Weapon;

            public virtual void Set(WeaponActionMode value) => Mode = value;
        }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor : Weapon.IProcessor
        {
            bool Input { get; }
        }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponActionMode>(this);
            Modules.Register(Weapon.Behaviours);

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Processor = Weapon.GetProcessor<IProcessor>(this);

            List = ComponentQuery.Collection.In<IState>(gameObject);
        }
        public override void Initialize()
        {
            base.Initialize();

            Weapon.OnProcess += Process;

            Index = 0;

            for (int i = 0; i < List.Count; i++)
            {
                if(List[i].enabled)
                {
                    Index = i;
                    break;
                }
            }

            if(List.Count > 0) Set(Index);
        }

        void Process()
        {
            if (Processor.Input)
            {
                if (CanChange)
                    Change();
            }
        }

        public virtual bool CanChange
        {
            get
            {
                if (Weapon.Constraint.Active) return false;

                if (enabled == false) return false;

                if (List.Count < 2) return false;

                return true;
            }
        }

        public delegate void ChangeDelegate(int index, IState module);
        public event ChangeDelegate OnChange;
        void Change()
        {
            Index++;

            if (Index >= List.Count) Index = 0;

            Set(Index);

            OnChange?.Invoke(Index, List[Index]);
        }
    }
}