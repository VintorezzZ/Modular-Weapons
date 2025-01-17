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
    public class WeaponRPM : Weapon.Module, WeaponConstraint.IInterface
    {
        [SerializeField]
        uint value = 800;
        public uint Value
        {
            get => value;
            set => this.value = value;
        }

        public float Delay => 60f / value;

        private float timer = 0f;

        public bool Active => timer > 0f;

        public Modifier.Scale Scale { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Scale = new Modifier.Scale();
        }

        public override void Initialize()
        {
            base.Initialize();

            Weapon.OnProcess += Process;
            Weapon.Action.OnPerform += Action;
        }

        void Process()
        {
            timer = Mathf.MoveTowards(timer, 0f,Time.deltaTime * Scale.Value);
        }

        void Action()
        {
            timer = Delay;
        }
    }
}