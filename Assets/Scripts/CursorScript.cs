using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public Texture2D[] cursors;
    // Start is called before the first frame update
    void Start()
    {
        defaulCursor();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void defaulCursor()
    {
        Cursor.SetCursor(cursors[0], Vector2.zero, CursorMode.ForceSoftware);
    }
}
