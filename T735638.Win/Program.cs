using System;
using System.Configuration;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
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
            T735638WindowsFormsApplication winApplication = new T735638WindowsFormsApplication();
            // Refer to the https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112680.aspx help article for more details on how to provide a custom splash form.
            //winApplication.SplashScreen = new DevExpress.ExpressApp.Win.Utils.DXSplashScreen("YourSplashImage.png");
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
            {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
            InMemoryDataStoreProvider.Register();
            winApplication.ConnectionString = InMemoryDataStoreProvider.ConnectionString;
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached && winApplication.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)
            {
                winApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
            try
            {
                winApplication.Setup();

                var listView = winApplication.CreateListView(typeof(Foo), false);

                winApplication.ShowViewStrategy.ShowView(new ShowViewParameters(listView)
                {
                    Context = TemplateContext.PopupWindow,
                    CreateAllControllers = true,
                    NewWindowTarget = NewWindowTarget.Separate,
                    TargetWindow = TargetWindow.NewModalWindow,
                }, new ShowViewSource(null, null));

                winApplication.Start();
            }
            catch (Exception e)
            {
                winApplication.HandleException(e);
            }
        }
    }
}
