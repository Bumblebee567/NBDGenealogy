using Caliburn.Micro;
using Db4objects.Db4o;
using NBDGenealogy.Helpers;
using NBDGenealogy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NBDGenealogy.ViewModels
{
    class SuccessorsTreeViewModel : Screen
    {
        private BindableCollection<PersonModel> _allPeopleInDatabase;
        private PersonModel _selectedPerson;
        private SuccessorsTreeItem _selectedPersonSuccessors;

        public SuccessorsTreeItem SelectedPersonSuccessors
        {
            get { return _selectedPersonSuccessors; }
            set
            {
                _selectedPersonSuccessors = value;
                NotifyOfPropertyChange(() => SelectedPersonSuccessors);
            }
        }


        public BindableCollection<PersonModel> AllPeopleInDatabase
        {
            get { return GetAllPeopleFormDatabase(); }
            set
            {
                _allPeopleInDatabase = value;
                NotifyOfPropertyChange(() => AllPeopleInDatabase);
            }
        }
        public PersonModel SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                NotifyOfPropertyChange(() => SelectedPerson);
            }
        }

        #region methods
        public BindableCollection<PersonModel> GetAllPeopleFormDatabase()
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            BindableCollection<PersonModel> allPeopleInDatabse = new BindableCollection<PersonModel>();
            var allPeople = db.QueryByExample(new PersonModel());
            foreach (var person in allPeople)
            {
                allPeopleInDatabse.Add((PersonModel)person);
            }
            db.Close();
            foreach (var person in allPeopleInDatabse)
            {
                if (person.BirthDate != null)
                    person.BirthDate.Value.ToShortDateString();
                if (person.DeathDate != null)
                    person.DeathDate.Value.ToShortDateString();
            }
            return allPeopleInDatabse;
        }
        public void ReturnSuccessorsTree()
        {
            if (SelectedPerson == null)
            {
                MessageBox.Show("Nie wybrano osoby", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                SuccessorsTreeItem root = SuccessorsTreeHelper.GetPersonsChildren(SelectedPerson.Name);
                SuccessorsTreeItem rootWithFirstPerson = new SuccessorsTreeItem { Title = "root"};
                rootWithFirstPerson.Items.Add(root);
                SelectedPersonSuccessors = rootWithFirstPerson;
                NotifyOfPropertyChange(() => SelectedPersonSuccessors);
            }
        }
        #endregion
    }
}
