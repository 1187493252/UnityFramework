using UnityEditor;
using TextEditor = UnityEditor.UI.TextEditor;

[CanEditMultipleObjects, CustomEditor(typeof(LinkSpriteText), false)]
public class LinkSpriteTextEditor : TextEditor
{
    private SerializedProperty _mInputText;
    private SerializedProperty _mOutputText;
    private SerializedProperty _mUseUniteLinkColor;
    private SerializedProperty _mUniteLinkColor;
    private SerializedProperty _mQuadUseFontSize;
    private SerializedProperty _mOffsetY;


    private SerializedProperty _mFontData;
    private SerializedProperty _mRaycastTarget;
    private SerializedProperty _mRaycastPadding;
    private SerializedProperty _mMaskable;


    protected override void OnEnable()
    {
        base.OnEnable();

        _mInputText = serializedObject.FindProperty("inputText");
        _mOutputText = serializedObject.FindProperty("_outputText");
        _mUseUniteLinkColor = serializedObject.FindProperty("m_UseUniteLinkColor");
        _mUniteLinkColor = serializedObject.FindProperty("m_UniteLinkColor");
        _mQuadUseFontSize = serializedObject.FindProperty("m_QuadUseFontSize");
        _mOffsetY = serializedObject.FindProperty("m_OffsetY");




        _mFontData = serializedObject.FindProperty("m_FontData");
        _mRaycastTarget = serializedObject.FindProperty("m_RaycastTarget");
        _mRaycastPadding = serializedObject.FindProperty("m_RaycastPadding");
        _mMaskable = serializedObject.FindProperty("m_Maskable");
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.PropertyField(_mInputText);
        //  EditorGUILayout.PropertyField(_mOutputText);
        EditorGUILayout.PropertyField(_mUseUniteLinkColor);
        EditorGUILayout.PropertyField(_mUniteLinkColor);
        EditorGUILayout.PropertyField(_mQuadUseFontSize);
        EditorGUILayout.PropertyField(_mOffsetY);


        EditorGUILayout.PropertyField(_mFontData);
        EditorGUILayout.PropertyField(_mRaycastTarget);
        EditorGUILayout.PropertyField(_mRaycastPadding);
        EditorGUILayout.PropertyField(_mMaskable);



        AppearanceControlsGUI();
        serializedObject.ApplyModifiedProperties();
    }
}
