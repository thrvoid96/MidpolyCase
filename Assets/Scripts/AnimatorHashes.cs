using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHashes : Singleton<AnimatorHashes>
{
    public static readonly int DoIdle = Animator.StringToHash("doIdle");
    public static readonly int DoRun = Animator.StringToHash("doRun");
    public static readonly int StopIdle = Animator.StringToHash("stopIdle");
}
