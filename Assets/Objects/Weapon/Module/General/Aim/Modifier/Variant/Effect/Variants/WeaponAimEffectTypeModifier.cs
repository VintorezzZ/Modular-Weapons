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
	public class WeaponAimEffectTypeModifier : WeaponAimEffectModifier
    {
        [SerializeField]
        protected WeaponEffects.TypeSelection selection;
        public WeaponEffects.TypeSelection Selection { get { return selection; } }

        public override bool IsTarget(WeaponEffects.IInterface effect) => selection.Type == effect.GetType();
    }
}