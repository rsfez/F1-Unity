using UnityEngine;

namespace Controllers.Builders
{
    public interface IControllerBuilder<out T> where T : IBuildableController
    {
        protected GameObject GetPrefabInstance();

        public T Build(params string[] args)
        {
            var instance = GetPrefabInstance();
            var controller = instance.GetComponent<T>();
            controller.Setup(args);
            return controller;
        }
    }
}