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
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : MonoBehaviour
    {
        public WeaponConstraint Constraint { get; protected set; }
        public WeaponAction Action { get; protected set; }
        public WeaponHit Hit { get; protected set; }
        public WeaponDamage Damage { get; protected set; }
        public WeaponOperation Operation { get; protected set; }
        public WeaponActivation Activation { get; protected set; }
        public WeaponPivot Pivot { get; protected set; }
        public WeaponEffects Effects { get; protected set; }
        public WeaponMesh Mesh { get; protected set; }

        #region Behaviours
        public Behaviours<Weapon> Behaviours { get; protected set; }

        public class Behaviour : MonoBehaviour, IBehaviour<Weapon>
        {
            new public bool enabled
            {
                get => base.enabled && gameObject.activeInHierarchy;
                set => base.enabled = value;
            }

            //To force the enabled tick box on the component to show
            protected virtual void Start() { }

            public virtual void Configure() { }
            public virtual void Init() { }

            public void CheckDependancy<TType>(TType instance)
            {
                if (instance == null)
                {
                    var exception = MUtility.FormatDependencyException<TType>(this);

                    throw exception;
                }
            }
        }
        #endregion

        #region Modules
        public Modules<Weapon> Modules { get; protected set; }

        public abstract class Module : Behaviour, IModule<Weapon>
        {
            public Weapon Weapon { get; protected set; }
            public virtual void Set(Weapon value) => Weapon = value;

            public IOwner Owner => Weapon.Owner;
        }
        #endregion

        public AudioSource AudioSource { get; protected set; }
        
        public IOwner Owner { get; protected set; }
        public virtual void Setup(IOwner owner)
        {
            this.Owner = owner;

            Configure();

            Init();
        }

        public interface IOwner
        {
            GameObject Root { get; }

            Damage.IDamager Damager { get; }

            List<IProcessor> Processors { get; }
        }

        public interface IProcessor
        {

        }

        public virtual TType GetProcessor<TType>(Component dependant)
                where TType : IProcessor
        {
            for (int i = 0; i < Owner.Processors.Count; i++)
                if (Owner.Processors[i] is TType target)
                    return target;

            throw MUtility.FormatDependencyException<TType>(dependant);
        }

        protected virtual void Configure()
        {
            AudioSource = GetComponent<AudioSource>();

            Behaviours = new Behaviours<Weapon>(this);

            Modules = new Modules<Weapon>(this);
            Modules.Register(Behaviours);

            Constraint = Modules.Depend<WeaponConstraint>();
            Action = Modules.Depend<WeaponAction>();
            Damage = Modules.Depend<WeaponDamage>();
            Hit = Modules.Depend<WeaponHit>();
            Operation = Modules.Depend<WeaponOperation>();
            Activation = Modules.Depend<WeaponActivation>();
            Pivot = Modules.Depend<WeaponPivot>();
            Effects = Modules.Depend<WeaponEffects>();
            Mesh = Modules.Depend<WeaponMesh>();

            Modules.Set();

            Behaviours.Configure();
        }

        protected virtual void Init()
        {
            Behaviours.Init();
        }

        public delegate void ProcessDelegate();
        public event ProcessDelegate OnProcess;
        public virtual void Process()
        {
            LatePerformCondition = true;

            OnProcess?.Invoke();
        }

        protected virtual void LateUpdate()
        {
            if (LatePerformCondition)
            {
                LateProcess();
                LatePerformCondition = false;
            }
        }
        bool LatePerformCondition;
        public event ProcessDelegate OnLateProcess;
        protected virtual void LateProcess()
        {
            OnLateProcess?.Invoke();
        }
        
        public virtual void Equip() => Activation.Enable();
        public virtual void UnEquip() => Activation.Disable();
    }
}