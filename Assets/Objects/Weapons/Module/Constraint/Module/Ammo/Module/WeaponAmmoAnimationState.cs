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
	public class WeaponAmmoAnimationState : WeaponAmmo.Module
	{
        public Animator Animator => Weapon.Mesh.Animator;

        public override void Init()
        {
            base.Init();

            Ammo.OnRefill += RefillCallback;
            Ammo.OnConsumption += ConsumptionCallback;

            Weapon.Activation.OnEnable += EnableCallback;
        }

        void EnableCallback() => UpdateState();

        protected virtual void UpdateState()
        {
            Animator.SetInteger("Remaining Uses", Ammo.Uses);
        }

        void RefillCallback() => UpdateState();
        void ConsumptionCallback() => UpdateState();
    }
}