using BitStrap;
using UnityEngine;

public class AttributesExample : MonoBehaviour
{
    [Header( "This is an example of the custom attributes in BitStrap." )]
    [LayerSelector]
    public int selectedLayer;

    [TagSelector]
    public string selectedTag;

    [ReadOnly]
    public int readOnlyInt = 7;

    [HelpBox( "This is a HelpBox.", HelpBoxAttribute.MessageType.Warning )]
    public int fieldWithHelpBox = 2;

    [Button]
    public void ButtonTest()
    {
        Debug.Log( "You pressed the button and executed a method." );
    }
}
