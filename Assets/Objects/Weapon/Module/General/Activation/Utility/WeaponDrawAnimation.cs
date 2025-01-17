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
	public class WeaponDrawAnimation : Weapon.Module, WeaponOperation.IInterface, WeaponConstraint.IInterface
	{
        public const string ID = "Draw";

        public WeaponMesh Mesh => Weapon.Mesh;

        public bool IsProcessing => Weapon.Operation.Is(this);

        public bool Active => IsProcessing;

        public override void Initialize()
        {
            base.Initialize();

            Weapon.Activation.OnEnable += Perform;

            Mesh.TriggerRewind.Register($"{ID} End", End);
        }

        public virtual void Perform()
        {
            Mesh.Animator.SetTrigger(ID);
            Weapon.Operation.Set(this);
        }

        protected virtual void End()
        {
            Weapon.Operation.Clear();
        }

        public virtual void Stop()
        {
            End();
        }
    }
}