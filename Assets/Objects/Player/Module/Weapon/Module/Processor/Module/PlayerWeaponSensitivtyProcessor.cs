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
	public class PlayerWeaponSensitivtyProcessor : PlayerWeapons.Processor, WeaponSensitivty.IProcessor
    {
        public float Scale { get; set; }

        public float Modifier() => Scale;

        public override void Initialize()
        {
            base.Initialize();

            Player.Controller.Look.Sensitivity.Scale.Add(Modifier);
        }
    }
}