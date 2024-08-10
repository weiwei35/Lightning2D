using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
[Preserve]
public class SkipUnityLogo : MonoBehaviour
{
    //启动画面前执行方法
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void Run(){
        Task.Run(() =>{
            SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
        });
    }
}
