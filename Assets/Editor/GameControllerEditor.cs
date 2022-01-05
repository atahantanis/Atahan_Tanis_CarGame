using System.Globalization;
using _Game.Scripts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GameController))]
    public class GameControllerEditor : UnityEditor.Editor
    {
        private readonly string[] _options = {"1", "2", "3", "4", "5", "6", "7", "8"};
        private const string ResetString = "Reset";

        private int _row;
        private int _lastIndex;
        private int _column;
        private int _groundMask;

        private Vector2 _lastStartingPos;
        private Vector2 _lastEndingPos;

        private Vector3 _groundTopYPosition;

        private GameController _myTarget;

        private GUIStyle _guiStyle;

        private SerializedProperty _serializedRow;
        private SerializedProperty _optionalMode;
        private void OnEnable()
        {
            _serializedRow = serializedObject.FindProperty("rows");
            _optionalMode = serializedObject.FindProperty("isOptionMode");
            _groundMask = LayerMask.NameToLayer("Ground");
            _myTarget = (GameController)target;
            _groundTopYPosition = new Vector3(0, _myTarget.transform.localScale.y/2, 0);
            _guiStyle = new GUIStyle();
            _guiStyle.normal.textColor = Color.cyan;
            _guiStyle.fontSize = 15;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("Select Car",_guiStyle);
            _row =EditorGUILayout.Popup(_row,_options);
            //After the selection of current car, always make sure to set the start position at first click for cars.
            if (_row != _lastIndex)
            {
                _lastIndex = _row;
                _column = 0;
            }
            serializedObject.Update();
            var columnArr = _serializedRow.GetArrayElementAtIndex(_row).FindPropertyRelative("column");
            _lastStartingPos = columnArr.GetArrayElementAtIndex(0).vector2Value;
            _lastEndingPos =  columnArr.GetArrayElementAtIndex(1).vector2Value;

            //Reset button
            if (GUILayout.Button(ResetString))
            {
                _row = 0;
                _column = 0;
                _lastStartingPos = Vector3.zero;
                _lastEndingPos = Vector3.zero;
                // _serializedRow.ClearArray(); removes the array elements, we just want set to zero them
                for (var i = 0; i < 8; i++)
                {
                    var columnArray = _serializedRow.GetArrayElementAtIndex(i).FindPropertyRelative("column");
                    columnArray.GetArrayElementAtIndex(0).vector2Value = new Vector2(0, 0);
                    columnArray.GetArrayElementAtIndex(1).vector2Value = new Vector2(0, 0);
                }
            }
            EditorGUILayout.LabelField("");
            //Optional Mode Toggle
            _optionalMode.boolValue = EditorGUILayout.Toggle("Remember Mode", _optionalMode.boolValue);
            // To save the changed data
            serializedObject.ApplyModifiedProperties();
            // To see the updated data on inspector.
            SceneView.RepaintAll();
        }

        private void OnSceneGUI()
        {
            //Current car information
            Handles.Label(_groundTopYPosition,(_row+1).ToString(CultureInfo.CurrentCulture),_guiStyle);

            //Green demonstrator box
            Handles.Label(_lastStartingPos + new Vector2(-4,2),"Starting Position",EditorStyles.boldLabel);
            Handles.color = Color.green;
            Handles.DotHandleCap(
                0,
                _lastStartingPos,
                Quaternion.identity,
                0.5f,
                EventType.Repaint
            );
            //Red demonstrator box
            Handles.Label(_lastEndingPos + new Vector2(-4,-0.5f),"Ending Position",EditorStyles.boldLabel);
            Handles.color = Color.red;
            Handles.DotHandleCap(
                0,
                _lastEndingPos,
                Quaternion.identity,
                0.5f,
                EventType.Repaint
            );

            //After left mouse click
            if (Event.current.type == EventType.MouseDown && Event.current.button==0)
            {
                var clickedPoint = (Vector2)HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
                //Make sure to get foremost objects' collider.
                var hitInfo = Physics2D.OverlapPoint(clickedPoint);
                if (hitInfo!=null && hitInfo.gameObject.layer == _groundMask)
                {
                    //Get the last data
                    serializedObject.Update();
                    //Set the starting and ending positions that exist at GameController, according to mouse clicks.
                    var columnArray = _serializedRow.GetArrayElementAtIndex(_row).FindPropertyRelative("column");
                    if (_column == 0)
                    {
                        columnArray.GetArrayElementAtIndex(_column).vector2Value = clickedPoint;
                        _column++;
                        _lastStartingPos = clickedPoint;
                    }
                    else
                    {
                        _lastEndingPos = clickedPoint;
                        columnArray.GetArrayElementAtIndex(_column).vector2Value = clickedPoint;
                        _column = 0;
                    }
                    serializedObject.ApplyModifiedProperties();
                }
                //To get the vector3 data precisely.
                /*Debug.Log(x1.ToString("F5"));*/
            }
        }
    }
}
