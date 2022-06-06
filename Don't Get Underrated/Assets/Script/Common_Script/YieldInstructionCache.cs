using System.Collections;
using System.Collections.Generic;
using UnityEngine;
internal static class YieldInstructionCache // ������ ���� ������ new WaitForEndOfFrame(),  new WaitForFixedUpdate(), new WaitForSeconds(seconds))�� �̸� ĳ���Ͽ�
                                            // ������ ������ �ø����� �ϴ� ��ũ��Ʈ
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
