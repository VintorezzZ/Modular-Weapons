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
	public class WeaponEffects : Weapon.Module
	{
        public List<IInterface> List { get; protected set; }
        public interface IInterface
        {
            bool enabled { get; set; }

            GameObject gameObject { get; }
            Transform transform { get; }

            Modifier.Scale Scale { get; }
        }

        public virtual int Count => List.Count;

        public virtual IInterface this[int index] => List[index];

        public delegate void RegisterDelegate(IInterface effect);
        public event RegisterDelegate OnRegister;
        public virtual void Register(IInterface element)
        {
            List.Add(element);

            OnRegister?.Invoke(element);
        }

        public override void Configure()
        {
            base.Configure();

            List = new List<IInterface>();
        }
    }
}