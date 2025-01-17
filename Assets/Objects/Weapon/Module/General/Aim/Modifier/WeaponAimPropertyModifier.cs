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
    public abstract class WeaponAimPropertyModifier : WeaponAim.Module, IModule<WeaponAimSight>
    {
        public virtual float Rate
        {
            get
            {
                switch (Effect)
                {
                    case EffectMode.Scaled:
                        return Aim.Rate * (Sight == null ? 1f : Sight.Weight);

                    case EffectMode.Constant:
                        return Sight == null ? 1 : Sight.Weight;
                }

                throw new NotImplementedException();
            }
        }

        public virtual EffectMode Effect => EffectMode.Scaled;
        public enum EffectMode
        {
            Scaled, Constant
        }

        [field: SerializeField, DebugOnly]
        public WeaponAimSight Sight { get; protected set; }
        public virtual void Set(WeaponAimSight reference)
        {
            Sight = reference;
        }

        protected virtual void Reset()
        {

        }
    }
}