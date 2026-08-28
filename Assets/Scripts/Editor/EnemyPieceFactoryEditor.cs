using Toolbox.Editor;
using UnityEditor;

namespace BlocksGame.Gameplay
{
    //Need this class to properly use ToolboxEditor for some reason
    [CustomEditor(typeof(EnemyPieceFactory))]
    public class EnemyPieceFactoryEditor : ToolboxEditor
    {

    }
}
