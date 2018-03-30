using Caliburn.Micro;
using NBDGenealogy.Models;
using NBDGenealogy.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBDGenealogy.ViewModels
{
    public class StartViewModel : Screen
    {
        public void OpenAddPerson()
        {
            var addPersonView = new AddPersonView();
            addPersonView.Show();
        }
    }
}
