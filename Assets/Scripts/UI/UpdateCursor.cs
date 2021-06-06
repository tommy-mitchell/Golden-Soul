using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCursor : MonoBehaviour
{
    private Image     swordDisplay;
    private Texture2D cursor;

    private Transform player;

    private float rotationAngle;
    private bool  cursorIsEnabled;
    private int   leftRightBool;

    private static float CURSOR_SIZE = 128f;

    private void Start()
    {
        cursorIsEnabled = true;
        leftRightBool   = 1;

        swordDisplay = GameObject.Find("Sword Display").GetComponent<Image>();
        cursor       = swordDisplay.sprite.texture;

        player = GameObject.Find("Player").transform;

        GetComponent<NewInput>().Player_onAim    += UpdateDirection;
        GetComponent<NewInput>().Player_onMove   += _input => { if (_input != 0) UpdateDirection(new Vector2(_input, 0)); }; // aim towards direction of movement
        GetComponent<NewInput>().controlsChanged += controlScheme => {
            cursorIsEnabled      = controlScheme == "Keyboard and Mouse";
            swordDisplay.enabled = controlScheme == "Gamepad";
        };
    }

    private void UpdateDirection(Vector2 aimDirection)
    {
        int yRotation;

        if(aimDirection.x == -.0001f)
        {
            aimDirection.x = 0;
            yRotation = 180;
        }
        else
            yRotation = aimDirection.x < 0 ? 180 : player.rotation.y == 0 ? 0 : 180;

        leftRightBool = yRotation == 0 ? 1 : -1;

        swordDisplay.transform.rotation = Quaternion.Euler(0, yRotation, aimDirection.y * 90);
        rotationAngle = aimDirection.y * -90 * leftRightBool;
    }

    private void OnGUI()
    {
        if(Cursor.visible)
            Cursor.visible = false;

        if(cursorIsEnabled)
        {
            float x = Event.current.mousePosition.x - CURSOR_SIZE / 2;
            float y = Event.current.mousePosition.y - CURSOR_SIZE / 2;

            GUIUtility.RotateAroundPivot(rotationAngle, new Vector2(x + CURSOR_SIZE / 2, y + CURSOR_SIZE / 2));
            GUI.DrawTexture(new Rect(x, y, leftRightBool * CURSOR_SIZE, CURSOR_SIZE), cursor);
        }
    }
}