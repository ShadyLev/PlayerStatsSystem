using System;
using UnityEngine;

namespace DependencyInjection
{
    /// <summary>
    /// Would love to use this not sure if I need it rn, fucking love dependency injection
    /// </summary>
    public class DependencyInjection
    {
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
        public sealed class InjectAttribute : PropertyAttribute { }
        
        [AttributeUsage(AttributeTargets.Method)]
        public sealed class ProvideAttribute : PropertyAttribute { }
        
        public interface IDependencyProvider { }
        
        
    }
}