using UnityEngine;
using UnityGameFramework.Runtime;

namespace NetworkBasedFPS
{
    public class MainMenuForm : UGuiForm
    {
        [SerializeField]
        private GameObject m_QuitButton = null;

        private ProcedureMainMenu m_ProcedureMainMenu = null;

        public void OnStartButtonClick()
        {
            //m_ProcedureMainMenu.StartGame();
        }

        public void OnSettingButtonClick()
        {
            //GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
        }

        public void OnAboutButtonClick()
        {
            //GameEntry.UI.OpenUIForm(UIFormId.AboutForm);
        }

        public void OnQuitButtonClick()
        {
            GameEntry.UI.OpenDialog(new DialogParams()
            {
                Mode = 2,
                Title = GameEntry.Localization.GetString("AskQuitGame.Title"),
                Message = GameEntry.Localization.GetString("AskQuitGame.Message"),
                OnClickConfirm = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); },
            });
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ProcedureMainMenu = (ProcedureMainMenu)userData;
            if (m_ProcedureMainMenu == null)
            {
                Log.Warning("ProcedureMainMenu is invalid when open MainMenuForm.");
                return;
            }

            m_QuitButton.SetActive(Application.platform != RuntimePlatform.IPhonePlayer);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            m_ProcedureMainMenu = null;

            base.OnClose(isShutdown, userData);
        }
    }
}
