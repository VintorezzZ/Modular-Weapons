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
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public class ControllerVelocity : FirstPersonController.Module
	{
        public Vector3 Absolute
        {
            get => rigidbody.velocity;
            set => rigidbody.velocity = value;
        }

        public Vector3 Relative
        {
            get => Controller.transform.InverseTransformDirection(Absolute);
            set => Absolute = Controller.transform.TransformDirection(value);
        }

        public Vector3 Forward => Calculate(Controller.transform.forward);
        public Vector3 Right => Calculate(Controller.transform.right);
        public Vector3 Up => Calculate(Controller.transform.up);

        public Rigidbody rigidbody => Controller.rigidbody;

        public virtual Vector3 Calculate(Vector3 direction)
        {
            return direction * Vector3.Dot(Absolute, direction);
        }
	}
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
}