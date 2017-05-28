using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class CTweenLibcs
{
    public enum LerpType {QuadEaseInOut, EaseIn, EaseOut, Elastic, Linear}

    private static Vector3 GetLerp(LerpType aLerpType, Vector3 start, Vector3 end, float time)
    {
        switch (aLerpType)
        {
            case LerpType.QuadEaseInOut:
                return Mathfx.Hermite(start, end, time);
            case LerpType.EaseIn:
                return Mathfx.Coserp(start, end, time);
            case LerpType.EaseOut:
                return Mathfx.Sinerp(start, end, time);
            case LerpType.Elastic:
                return Mathfx.Berp(start, end, time);
            case LerpType.Linear:
                return new Vector3(Mathf.Lerp(start.x, end.x, time), Mathfx.Lerp(start.y, end.y, time), Mathfx.Lerp(start.z, end.z, time));
            default:
                return Vector2.zero;
        }
    }

    public static float GetLerp(LerpType aLerpType, float start, float end, float time)
    {
        switch (aLerpType)
        {
            case LerpType.QuadEaseInOut:
                return Mathfx.Hermite(start, end, time);
            case LerpType.EaseIn:
                return Mathfx.Coserp(start, end, time);
            case LerpType.EaseOut:
                return Mathfx.Sinerp(start, end, time);
            case LerpType.Elastic:
                return Mathfx.Berp(start, end, time);
            case LerpType.Linear:
                return Mathfx.Lerp(start, end, time);
            default:
                return 0f;
        }
    }

    static IEnumerator DoMoveCoroutine(GameObject m, Vector3 aDestination, float aTime, bool local = false, LerpType aLerpType = LerpType.Linear, Coroutine aCorout = null)
    {
        float startTime = Time.time;
        Vector3 startPos = (local ? m.transform.localPosition : m.transform.position);
        while (Time.time - startTime < aTime)
        {
            float t = (Time.time - startTime) / aTime;
            if (local)
                m.transform.localPosition = GetLerp(aLerpType, startPos, aDestination, aTime);
            else
                m.transform.position = GetLerp(aLerpType, startPos, aDestination, aTime);
            yield return null;
        }
        aCorout = null;
    }

    static IEnumerator DoFadeSpriteCoroutine(GameObject m, SpriteRenderer aRend, Vector4 origin, float targetAlpha, float aTime, LerpType aLerpType = LerpType.Linear, Coroutine aCoroutine = null)
    {
        float startTime = Time.time;
        while (Time.time - startTime < aTime)
        {
            float t = (Time.time - startTime) / aTime;
            aRend.color = new Vector4(origin.x,origin.y, origin.z, GetLerp(aLerpType, origin.z, targetAlpha, aTime));
            yield return null;
        }
        aCoroutine = null;
    }

    /// <summary>
    /// Moves the GameObject from current position to aDestination, the Interpolation method by default is Linear.
    /// </summary>
    /// <returns></returns>
    public static GameObject DoMove(this GameObject m, Vector3 aDestination, float aTime, LerpType aLerpType = LerpType.Linear, bool local = false , System.Action aAction = null)
    {
        CGameObject ownGameObj = m.GetComponent<CGameObject>();
        ownGameObj._WorkingCoroutines.Add(
            m.GetComponent<MonoBehaviour>().StartCoroutine(DoMoveCoroutine(m, aDestination, aTime, local, aLerpType, ownGameObj._WorkingCoroutines[ownGameObj._WorkingCoroutines.Count - 1])));
        return m;
    }

    /// <summary>
    /// Interpolates de values of a given SpriteRenderer from a value to another. The default interpolation method is Linear.
    /// </summary>
    /// <returns></returns>
    public static GameObject DoFadeSprite(this GameObject m, SpriteRenderer aRend,float fromVal, float toVal, float aTime, LerpType aLerpType = LerpType.Linear, System.Action aAction = null)
    {
        if (aRend != null)
        {
            CGameObject ownGameObj = m.GetComponent<CGameObject>();
            Vector4 startColor = aRend.color;
            startColor.w = fromVal;
            ownGameObj._WorkingCoroutines.Add(
                m.GetComponent<MonoBehaviour>().StartCoroutine(DoFadeSpriteCoroutine(m, aRend, startColor, toVal, aTime, aLerpType, ownGameObj._WorkingCoroutines[ownGameObj._WorkingCoroutines.Count - 1])));
        }
        return m;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="m"></param>
    /// <returns></returns>
    public static GameObject DoGoStayAndBack(this GameObject m)
    {
        return m;
    }
}

