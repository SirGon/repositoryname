using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TrackSnapping : MonoBehaviour
{
    [MenuItem("Track Tools/Snap")]
    static void SnapTrack ()
    {
        if (Selection.gameObjects.Length != 2)
        {
            Debug.LogWarning("You must select exactly 2 track chunks.");
            return;
        }
        GameObject chunkA = Selection.activeGameObject;
        GameObject chunkB = Selection.gameObjects[0] == Selection.activeGameObject ? Selection.gameObjects[1] : Selection.gameObjects[0];

        Transform[] chunkA_anchors = new Transform[2];
        Transform[] chunkB_anchors = new Transform[2];

        chunkA_anchors[0] = chunkA.transform.FindChild("Anchor00");
        chunkA_anchors[1] = chunkA.transform.FindChild("Anchor01");

        chunkB_anchors[0] = chunkB.transform.FindChild("Anchor00");
        chunkB_anchors[1] = chunkB.transform.FindChild("Anchor01");

        float minDistance = Mathf.Infinity;
        Vector3 delta = Vector3.zero;

        for (int i = 0; i < chunkA_anchors.Length; i++)
        {
            for (int x = 0; x < chunkB_anchors.Length; x++)
            {
                if (chunkA_anchors[i] == null || chunkB_anchors[x] == null)
                {
                    Debug.LogWarning("One of the two chunk miss an Anchor point.");
                    break;
                }
                Vector3 _delta = chunkA_anchors[i].position - chunkB_anchors[x].position;
                if (_delta.sqrMagnitude < minDistance)
                {
                    minDistance = _delta.sqrMagnitude;
                    delta = _delta;
                }
            }
        }

        chunkB.transform.position += delta;
    }
}
