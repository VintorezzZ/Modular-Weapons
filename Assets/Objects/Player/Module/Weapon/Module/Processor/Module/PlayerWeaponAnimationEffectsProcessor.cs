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
    public class PlayerWeaponAnimationEffectsProcessor : PlayerWeapons.Processor, WeaponAnimationEffects.IProcessor
    {
        public FirstPersonController Controller => Player.Controller;

        public event WeaponAnimationEffects.JumpDelegate OnJump;

        public event WeaponAnimationEffects.LeaveGroundDelegate OnLeaveGround;

        public event WeaponAnimationEffects.LandDelegate OnLand;

        public override void Initialize()
        {
            base.Initialize();

            Controller.Jump.OnPerform += JumpCallback;
            Controller.Ground.Change.OnLeave += LeaveGroundCallback;
            Controller.Ground.Change.OnLand += LandCallback;
        }

        void JumpCallback() => OnJump?.Invoke(Controller.Jump.Count);
        void LeaveGroundCallback() => OnLeaveGround?.Invoke();
        void LandCallback(ControllerAirTravel.Data travel) => OnLand?.Invoke(Controller.Velocity.Relative);
    }
}