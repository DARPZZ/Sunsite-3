using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Sunsite_3.ViewModel;
using System.Windows.Controls;

namespace Sunsite_3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ContentControl ContentControlRef { get; set; } = null;

        public App()
        {
            ServiceContainer.Register(() => new HomeViewModel());
            ServiceContainer.Register(() => new MessageViewModel());
            ServiceContainer.Register(() => new SettingsViewModel());
            ServiceContainer.Register(() => new PostViewModel());

        }

        public static Share Sharedata { get; } = new Share();

        public void ChangeUserControl(Type viewModelType)
        {
            UserControl tmpUC = CreatePage(viewModelType, null);

            var viewModel = ServiceContainer.Resolve(viewModelType);
            //view.BindingContext = viewModel;

            tmpUC.DataContext = viewModel;
            this.ContentControlRef.Content = tmpUC;

            //return CreatePage(viewModelType, null);
        }

        /// <summary>
        /// This metod will use the naming convention between the ViewModel and the View
        /// To find the name of the View that matches the ViewModel
        /// The view name is then used to find the type (the view class name)
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <returns></returns>
        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private UserControl CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }
            //CreateInstance, will create an instance based on the type, the default constructor is always invoked
            UserControl page = Activator.CreateInstance(pageType) as UserControl;

            return page;
        }

    }
}

