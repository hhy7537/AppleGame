using UnityEngine;

namespace AppleGame.Util
{
    public class BehaviourBase : MonoBehaviour
    {
#if UNITY_EDITOR
        public void BindSerializedField()
        {
            OnBindSerialzedField();
        }

        protected virtual void OnBindSerialzedField() { }
#endif

#if UNITY_EDITOR
        [UnityEditor.CustomEditor(typeof(BehaviourBase), true)]
        public class SerializeFieldEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                BehaviourBase behaviourBase = (BehaviourBase)target;

                if (GUILayout.Button("Bind Serialized Field"))
                {
                    behaviourBase.BindSerializedField();
                    UnityEditor.EditorUtility.SetDirty(behaviourBase);
                }

                DrawDefaultInspector();
            }
        }
#endif
    }
}