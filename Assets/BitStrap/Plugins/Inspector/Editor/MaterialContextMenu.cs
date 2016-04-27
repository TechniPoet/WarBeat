﻿using UnityEditor;

public class MaterialContextMenu
{
    [MenuItem( "CONTEXT/Material/Select Material" )]
    public static void SelectMaterial( MenuCommand command )
    {
        Selection.activeObject = command.context;
    }
}
