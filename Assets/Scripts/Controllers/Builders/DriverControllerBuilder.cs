using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Controllers.Builders
{
    public class DriverControllerBuilder : IControllerBuilder<DriverController>
    {
        private const string Path = "Prefabs/Driver";

        private static readonly Lazy<DriverControllerBuilder> LazyBuilderInstance =
            new(() => new DriverControllerBuilder());

        public static IControllerBuilder<DriverController> Instance => LazyBuilderInstance.Value;

        GameObject IControllerBuilder<DriverController>.GetPrefabInstance()
        {
            return Object.Instantiate(Resources.Load(Path) as GameObject);
        }
    }
}