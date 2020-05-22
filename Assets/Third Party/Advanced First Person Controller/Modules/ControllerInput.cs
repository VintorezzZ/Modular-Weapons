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
	public class ControllerInput : FirstPersonController.Module
	{
        public Vector2 Move { get; protected set; }

        public Vector2 Look { get; protected set; }

        public ButtonInput Jump { get; protected set; }

        public SprintInput Sprint { get; protected set; }

        [Serializable]
        public class SprintInput
        {
            public float Axis { get; protected set; }

            public ButtonInput Button { get; protected set; }

            public virtual void Process(float axis)
            {
                this.Axis = axis;

                Button.Process(axis > 0f);
            }

            public SprintInput()
            {
                Button = new ButtonInput();
            }
        }

        public ButtonInput Crouch { get; protected set; }

        public ButtonInput Prone { get; protected set; }

        public override void Configure(FirstPersonController reference)
        {
            base.Configure(reference);

            Jump = new ButtonInput();

            Sprint = new SprintInput();

            Crouch = new ButtonInput();

            Prone = new ButtonInput();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Move = GetAxes("Move");

            Look = GetAxes("Look");

            Jump.Process(GetKey(KeyCode.Space, KeyCode.JoystickButton0));

            Sprint.Process(GetAxis("Sprint"));

            Crouch.Process(GetKey(KeyCode.C, KeyCode.JoystickButton1));

            Prone.Process(GetKey(KeyCode.LeftControl, KeyCode.JoystickButton2));
        }

        protected virtual Vector2 GetAxes(string name)
        {
            var x = GetAxis(name + " X");
            var y = GetAxis(name + " Y");

            return new Vector2(x, y);
        }
        protected virtual float GetAxis(string name)
        {
            var keyboard = Input.GetAxisRaw(name + " - PC");

            var joystick = Input.GetAxisRaw(name + " - Joystick");

            return keyboard + joystick;
        }

        protected virtual bool GetKey(params KeyCode[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
                if (Input.GetKey(keys[i]))
                    return true;

            return false;
        }
    }
}