using System;
using System.Configuration;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.XtraEditors;
using T735638.Module.BusinessObjects;

namespace T735638.Win
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if EASYTEST
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif
            WindowsFormsSettings.LoadApplicationSettings();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            if (Tracing.GetFileLocationFromSettings() == DevExpress.Persistent.Base.FileLocation.CurrentUserApplicationDataFolder)
            {
                Tracing.LocalUserAppDataPath = Application.LocalUserAppDataPath;
            }
            Tracing.Initialize();
            var winApplication = new T735638WindowsFormsApplication();
            // Refer to the https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112680.aspx help article for more details on how to provide a custom splash form.
            //winApplication.SplashScreen = new DevExpress.ExpressApp.Win.Utils.DXSplashScreen("YourSplashImage.png");
            InMemoryDataStoreProvider.Register();
            winApplication.ConnectionString = InMemoryDataStoreProvider.ConnectionString;

            if (System.Diagnostics.Debugger.IsAttached && winApplication.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)
            {
                winApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
            try
            {
                winApplication.Setup();

                var listView = winApplication.CreateListView(typeof(Foo), false);

                var dialogController = winApplication.CreateController<CustomDialogController>();

                dialogController.Accepting +=(s,e) =>
                {

                };

                dialogController.Cancelling +=(s,e) =>
                {

                };

                winApplication.SplashScreen.Stop();
                winApplication.ShowViewStrategy.ShowView(new ShowViewParameters(listView)
                {
                    Context = TemplateContext.PopupWindow,
                    CreateAllControllers = true,
                    NewWindowTarget = NewWindowTarget.Separate,
                    TargetWindow = TargetWindow.NewModalWindow,
                    Controllers = { dialogController }
                }, new ShowViewSource(null, null));
            }
            catch (Exception e)
            {
                winApplication.HandleException(e);
            }
        }
    }

    public class CustomDialogController : DialogController
    {
        protected override void OnFrameAssigned()
        {
            base.OnFrameAssigned();
            Frame.TemplateChanged += TemplateChanged;
        }

        private void TemplateChanged(object sender, EventArgs e)
        {
            if(Frame.Template is System.Windows.Forms.Form)
            {
                var form = Frame.Template as System.Windows.Forms.Form;
                form.ControlBox = false;
                form.WindowState = FormWindowState.Maximized;
            }
        }
    }
}
