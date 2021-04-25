using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectPool : MonoBehaviour
{
    void Start()
    {
        ObjectPoolingManager.MyInstance.SetObject("GrenadeExplosionFire", 2);
        ObjectPoolingManager.MyInstance.SetObject("NovaFireRed", 2);
        ObjectPoolingManager.MyInstance.SetObject("NovaFireRed_Small", 2);
        ObjectPoolingManager.MyInstance.SetObject("CartoonyBodySlam", 2);
        ObjectPoolingManager.MyInstance.SetObject("CartoonyBodySlam_Small", 6);
        ObjectPoolingManager.MyInstance.SetObject("IntroDust_Brown", 1);
        ObjectPoolingManager.MyInstance.SetObject("Dust", 2);
        ObjectPoolingManager.MyInstance.SetObject("ROCK", 2);
        ObjectPoolingManager.MyInstance.SetObject("vfx_MagicAbility_Impact_Ground", 1);
        ObjectPoolingManager.MyInstance.SetObject("DustDirtyPoofSoft_Brown", 6);
        ObjectPoolingManager.MyInstance.SetObject("DustDirtyPoofSoft_Big_Brown", 1);
        ObjectPoolingManager.MyInstance.SetObject("ExplosionNovaFire", 10);
        ObjectPoolingManager.MyInstance.SetObject("fire-tornado", 4);
        
        ObjectPoolingManager.MyInstance.SetObject("TargetHitExplosion", 15);
    }
}
