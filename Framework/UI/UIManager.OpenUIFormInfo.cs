/*
* FileName:          UIManager.OpenUIFormInfo
* CompanyName:       
* Author:            relly
* Description:       
*/



namespace Framework.UI
{
    internal sealed partial class UIManager : FrameworkModule, IUIManager
    {
        private sealed class OpenUIFormInfo : IReference
        {
            private int m_Id;
            private UIGroup m_UIGroup;
            private bool m_PauseCoveredUIForm;
            private object m_UserData;

            public OpenUIFormInfo()
            {
                m_Id = 0;
                m_UIGroup = null;
                m_PauseCoveredUIForm = false;
                m_UserData = null;
            }

            public int Id
            {
                get
                {
                    return m_Id;
                }
            }

            public UIGroup UIGroup
            {
                get
                {
                    return m_UIGroup;
                }
            }

            public bool PauseCoveredUIForm
            {
                get
                {
                    return m_PauseCoveredUIForm;
                }
            }

            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }

            public static OpenUIFormInfo Create(int serialId, UIGroup uiGroup, bool pauseCoveredUIForm, object userData)
            {
                OpenUIFormInfo openUIFormInfo = ReferencePool.Acquire<OpenUIFormInfo>();
                openUIFormInfo.m_Id = serialId;
                openUIFormInfo.m_UIGroup = uiGroup;
                openUIFormInfo.m_PauseCoveredUIForm = pauseCoveredUIForm;
                openUIFormInfo.m_UserData = userData;
                return openUIFormInfo;
            }

            public void Clear()
            {
                m_Id = 0;
                m_UIGroup = null;
                m_PauseCoveredUIForm = false;
                m_UserData = null;
            }
        }
    }
}
