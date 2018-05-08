using Caliburn.Micro;
using Db4objects.Db4o;
using NBDGenealogy.Helpers;
using NBDGenealogy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NBDGenealogy.ViewModels
{
    public class StartViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public StartViewModel()
        {
            ActivateItem(new AddPersonViewModel());
        }
        public void LoadAddPersonView()
        {
            ActivateItem(new AddPersonViewModel());
        }
        public void LoadModifyPersonView()
        {
            ActivateItem(new ModifyPersonViewModel());
        }
        public void LoadCommonAncestorsView()
        {
            ActivateItem(new CommonAncestorsViewModel());
        }
        public void LoadDeletePersonView()
        {
            ActivateItem(new DeletePersonViewModel());
        }
        public void LoadShowSuccessorsView()
        {
            ActivateItem(new ShowSuccessorsViewModel());
        }
        public void LoadSuccessorsTreeView()
        {
            ActivateItem(new SuccessorsTreeViewModel());  
        }
    }
}
