﻿using System;
using System.Reflection;
using Live2k.Core.Model;
using Live2k.Core.Model.Base;
using Live2k.Core.Model.Basic.Commodities;

namespace Live2k.Core.Utilities
{
    public class Factory
    {
        private readonly Mediator _mediator;

        public Factory(Mediator mediator)
        {
            this._mediator = mediator;
        }

        public T CreateNew<T>(string label, string description, params Tuple<string, object>[] properties) where T: Entity
        {
            var instance = CreateNew<T>();
            instance.Label = label;
            instance.Description = description;
            foreach (var prop in properties)
            {
                instance[prop.Item1] = prop.Item2;
            }
            CallArgumentLessMethod(instance, "GenerateLabel");

            // if instance is a RevisableCommodity, call Revise method
            if (instance is RevisableCommodity)
                (instance as RevisableCommodity).Revise();

            // if instance is a node, call AttachTracker method
            if (instance is Node)
                (instance as Node).AttachTracker(instance as Node, null);

            return instance;
        }

        private void CallArgumentLessMethod<T>(T instance, string name) where T : Entity
        {
            var method = typeof(T).GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
            method.Invoke(instance, new object[0]);
        }

        public T CreateNew<T>() where T: Entity
        {
            // Get constructor accepting mediator
            var constructor = GetConstructor(typeof(T)) ??
                throw new TypeAccessException($"Could not find proper constructor for {typeof(T)}");
            var instance = constructor.Invoke(new object[] { this._mediator }) ?? 
                throw new OperationCanceledException($"Could not instanciate {typeof(T)}");
            return instance as T;
        }

        private ConstructorInfo GetConstructor(Type type)
        {
            return type.GetConstructor(BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[] { typeof(Mediator) },
                null);
        }

    }
}
