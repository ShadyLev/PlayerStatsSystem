using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Code.Environment
{
    public class WaterEffectTrigger : MonoBehaviour
    {
        CustomPassVolume _waterEffectVolume;
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player") == false)
                return;
            
            
        }
    }
}