using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputChecker : MonoBehaviour
{
    // bool keys[]
    // bool mouse[]
    // Point cursorPos

    // bool checkKey(int keyID)
    // bool checkMouse(int mouseID)
    // Point getCursorPos()

    Vector2 cursorPos;

    private void Update()
    {
        cursorPos = Input.mousePosition; // get angle between this and player's pos
    }

    // angle: Vector2.SignedAngle(player.pos, cursorPos)
}
