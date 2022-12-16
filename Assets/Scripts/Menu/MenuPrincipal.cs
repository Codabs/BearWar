using SceneControllerMD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuPrincipal : MonoBehaviour
{
    public void Quit() => Application.Quit(); // Quit the game

    public Texture2D cursor;
    void Awake()
    {
        Cursor.SetCursor(cursor, new Vector2(26, 0), CursorMode.Auto);
    }
}
