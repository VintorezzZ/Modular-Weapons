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
	public abstract class BaseWeaponReload : Weapon.Module, WeaponConstraint.IInterface, WeaponOperation.IInterface
	{
        public bool IsProcessing => Equals(Weapon.Operation.Value);
        bool WeaponConstraint.IInterface.Constraint => IsProcessing;

        [SerializeField]
        protected bool auto = true;
        public bool Auto { get { return auto; } }

        public WeaponAmmo Ammo { get; protected set; }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Ammo = Weapon.GetDependancy<WeaponAmmo>();
        }

        public override void Init()
        {
            base.Init();

            if(Ammo == null)
            {
                Debug.LogError(FormatDependancyError<WeaponAmmo>());
                enabled = false;
                return;
            }

            Weapon.OnLateProcess += LateProcess;

            Weapon.Action.OnLatePerform += LateAction;
        }
        
        void LateProcess(Weapon.IProcessData data)
        {
            if (data is IData)
                LateProcess(data as IData);
        }
        void LateProcess(IData data)
        {
            if (data.Input)
            {
                if (CanPerform)
                    Perform();
            }
        }

        void LateAction()
        {
            if (auto)
            {
                if (Ammo.CanConsume == false && CanPerform)
                    Perform();
            }
        }

        public bool CanPerform
        {
            get
            {
                if (Ammo.Magazine.IsFull) return false;

                if (Ammo.Reserve.IsEmpty) return false;

                if (IsProcessing) return false;

                return true;
            }
        }
        public virtual void Perform()
        {
            Weapon.Operation.Set(this);
        }

        protected virtual void Complete()
        {
            End();

            Ammo.Refill();
        }

        protected virtual void End()
        {
            Weapon.Operation.Clear();
        }

        public virtual void Stop()
        {
            //TODO provide functionality to stop reload

            End();
        }

        public interface IData
        {
            bool Input { get; }
        }
    }

    public class WeaponReload : BaseWeaponReload
    {
        public override void Perform()
        {
            base.Perform();

            StartCoroutine(Procedure());
        }

        IEnumerator Procedure()
        {
            yield return new WaitForSeconds(1f);

            Complete();
        }
    }
}