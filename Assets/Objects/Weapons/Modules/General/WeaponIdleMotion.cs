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
	public class WeaponIdleMotion : Weapon.Module, Weapon.IEffect
	{
        [SerializeField]
        protected Transform context;
        public Transform Context { get { return context; } }

        [SerializeField]
        protected float scale = 1f;
        public float Scale
        {
            get => scale;
            set => scale = value;
        }

        [SerializeField]
        protected float speed = 2f;
        public float Speed { get { return speed; } }

        [SerializeField]
        protected float range = 0.002f;
        public float Range { get { return range; } }

        [SerializeField]
        Vector3 axis = Vector3.up;

        public Vector3 Offset { get; protected set; }

        protected virtual void Reset()
        {
            context = transform;
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnLateProcess += LateProcess;
        }

        void LateProcess(Weapon.IProcessData data)
        {
            context.localPosition -= Offset;

            Offset = range * Mathf.Sin(speed * Time.time) * axis * scale;

            context.localPosition += Offset;
        }
    }
}