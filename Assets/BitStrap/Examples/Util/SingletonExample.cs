using BitStrap;
using UnityEngine;

public class SingletonExample : MonoBehaviour
{
    [Button]
    public void AccessDummySingletonFieldDirectly()
    {
        if( Application.isPlaying )
        {
            var fieldValue = Singleton<DummySingleton>.Instance.dummyIntField;
            Debug.LogFormat( "Accessing DummySingleton's dummyIntField directly: {0}", fieldValue );
        }
        else
        {
            Debug.LogWarning( "In order to see Singleton working, please enter Play mode." );
        }
    }
}
