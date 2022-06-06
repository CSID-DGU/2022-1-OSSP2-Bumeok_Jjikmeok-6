using System.Collections;
using System.Collections.Generic;
using UnityEngine;
internal static class YieldInstructionCache // 성능이 많이 느리는 new WaitForEndOfFrame(),  new WaitForFixedUpdate(), new WaitForSeconds(seconds))를 미리 캐싱하여
                                            // 게임의 성능을 올리도록 하는 스크립트
{
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    private static readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if (!waitForSeconds.TryGetValue(seconds, out wfs))
            waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }
}
