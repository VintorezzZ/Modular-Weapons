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
	public class WeaponAimSpeed : WeaponAim.Module
	{
        [SerializeField]
        protected float _base = 5f;
        public float Base { get { return _base; } }

        public float Value { get; protected set; }

        public Modifier.Scale Scale { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Scale = new Modifier.Scale();

            Calculate();
        }

        public override void Initialize()
        {
            base.Initialize();

            Weapon.OnProcess += Process;
        }

        void Process()
        {
            Calculate();
        }

        protected virtual void Calculate()
        {
            Value = Base * Scale.Value;
        }
    }
}