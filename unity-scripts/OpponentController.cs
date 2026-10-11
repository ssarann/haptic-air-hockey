using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OpponentController : MonoBehaviour
{
    [Tooltip("1 = enako kot marker, <1 = počasnejši, >1 = hitrejši premiki")]
    public float movementScale = 1f;

    Vector3 initialMarkerPos;
    Vector3 initialLocalPos;
    Quaternion initialLocalRot;
    bool calibrated = false;

    void Start()
    {
        // tu smo na Nasprotnik objektu, njegovo parent je vedno ImageTarget
        if (transform.parent != null)
        {
            initialMarkerPos = transform.parent.position;
            initialLocalPos = transform.localPosition;
            initialLocalRot = transform.localRotation;
            calibrated = true;
        }
        else Debug.LogError("OpponentController mora biti na deli ImageTargeta!");
    }

    void LateUpdate()
    {
        if (!calibrated) return;

        // markerjeva pozicija WORLD SPACE
        Vector3 curMarker = transform.parent.position;

        // premik markerja od starta
        Vector3 delta = curMarker - initialMarkerPos;

        // skaliraj in zamrzni Y na 0
        delta *= movementScale;
        delta.y = 0;

        // nova lokalna pozicija otroka
        transform.localPosition = initialLocalPos + delta;

        // zakleni rotacijo, da se ne vrti okoli Y ali X/Z
        transform.localRotation = initialLocalRot;
    }
}
