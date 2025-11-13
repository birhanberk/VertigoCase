using UnityEditor;

namespace Editor
{
    public class WheelEditorSettingsPanel : IWheelEditorPanel
    {
        private readonly WheelEditorToolWindow window;

        public WheelEditorSettingsPanel(WheelEditorToolWindow window)
        {
            this.window = window;
        }

        public void Draw()
        {
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space(10);

            EditorGUI.BeginChangeCheck();

            window.levelPath = EditorGUILayout.TextField("Level Path", window.levelPath);
            window.zonePath  = EditorGUILayout.TextField("Zone Path", window.zonePath);
            window.itemPath  = EditorGUILayout.TextField("Item Path", window.itemPath);
            window.iconPath  = EditorGUILayout.TextField("Icon Path", window.iconPath);

            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString("VertigoTool_LevelPath", window.levelPath);
                EditorPrefs.SetString("VertigoTool_ZonePath",  window.zonePath);
                EditorPrefs.SetString("VertigoTool_ItemPath",  window.itemPath);
                EditorPrefs.SetString("VertigoTool_IconPath",  window.iconPath);

                window.RefreshZoneList();
                window.LoadLevelSettings();
                window.CreateReorderableList();
            }
        }
    }
}