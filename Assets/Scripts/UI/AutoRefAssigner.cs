using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class AutoRefAssigner : MonoBehaviour
    {
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (Application.isPlaying) return;
            AutoAssignButtons();
        }

        private void AutoAssignButtons()
        {
            var buttons = GetComponentsInChildren<Button>(true);
            var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            var changed = false;

            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<AutoButtonAttribute>();
                if (attr == null) continue;

                foreach (var btn in buttons)
                {
                    if (btn.name.ToLower().Contains(attr.Keyword.ToLower()))
                    {
                        field.SetValue(this, btn);
                        changed = true;
                        break;
                    }
                }
            }
            if (changed)
            {
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}