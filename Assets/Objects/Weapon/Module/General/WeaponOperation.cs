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
	public class WeaponOperation : Weapon.Module
	{
        public IInterface Value { get; protected set; }
        public interface IInterface
        {
            void Stop();
        }

        public virtual void Set(IInterface target)
        {
            if (Value != null) Value.Stop();

            Value = target;
        }

        public virtual bool Is(IInterface target)
        {
            return target == Value;
        }

        public virtual void Clear(IInterface target)
        {
            if (target == Value) Clear();
        }
        public virtual void Clear()
        {
            Value = null;
        }
    }
}