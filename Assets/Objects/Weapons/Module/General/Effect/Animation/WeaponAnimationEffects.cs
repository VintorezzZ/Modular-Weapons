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
	public class WeaponAnimationEffects : Weapon.Module<WeaponAnimationEffects.IProcessor>, Weapon.IEffect
	{
        [SerializeField]
        protected float scale;
        public float Scale
        {
            get => scale;
            set => scale = value;
        }

        public WeaponMesh Mesh => Weapon.Mesh;
        public Animator Animator => Mesh.Animator;

        public WeaponAnimationEffectsWeight Weight { get; protected set; }

        public class Module : Weapon.BaseModule<WeaponAnimationEffects, IProcessor>
        {
            public WeaponAnimationEffects Effects => Reference;

            public override Weapon Weapon => Effects.Weapon;

            public WeaponMesh Mesh => Weapon.Mesh;
            public Animator Animator => Mesh.Animator;
        }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Weight = Dependancy.Get<WeaponAnimationEffectsWeight>(gameObject);

            References.Configure(this);
        }

        public override void Init()
        {
            base.Init();

            References.Init(this);
        }

        public interface IProcessor
        {
            event JumpDelegate OnJump;

            event LandDelegate OnLand;
        }

        public delegate void JumpDelegate(int count);

        public delegate void LandDelegate(Vector3 relativeVelocity);
    }
}