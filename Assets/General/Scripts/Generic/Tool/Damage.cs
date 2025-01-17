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
    public static class Damage
    {
        public interface IInterface
        {
            GameObject gameObject { get; }
        }

        public interface IDamagable : IInterface
        {
            Result Take(IDamager source, Request request);
        }

        public interface IDamager : IInterface
        {
            Result Perform(IDamagable target, Request request);
        }

        public static Result Invoke(IDamager source, IDamagable target, Request request)
        {
            return target.Take(source, request);
        }

        public struct Request
        {
            public float Value { get; set; }
            public Method Method { get; set; }

            public Request(float value, Method method)
            {
                this.Value = value;
                this.Method = method;
            }
        }

        public struct Result
        {
            public IDamager Source { get; private set; }

            public IDamagable Target { get; private set; }

            public float Value { get; private set; }

            public Method Method { get; private set; }

            public Result(IDamager source, IDamagable target, Request request)
            {
                this.Source = source;
                this.Target = target;
                this.Value = request.Value;
                this.Method = request.Method;
            }
        }

        public enum Method
        {
            Undefined, Contact, Projectile
        }
    }
}