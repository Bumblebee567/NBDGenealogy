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
    class CommonAncestorsViewModel : Screen
    {
        public CommonAncestorsViewModel()
        {
            _firstPersonList = GetAllPeopleFormDatabase();
            _secondPersonList = GetAllPeopleFormDatabase();
        }
        private List<PersonModel> _firstPersonList;
        private List<PersonModel> _secondPersonList;
        private PersonModel _firstListSelectedPerson;
        private PersonModel _secondListSelectedPerson;
        private string _commonAncestorsList;

        public string CommonAncestorsList
        {
            get { return _commonAncestorsList; }
            set
            {
                _commonAncestorsList = value;
                NotifyOfPropertyChange(() => CommonAncestorsList);
            }
        }


        public PersonModel SecondListSelectedPerson
        {
            get { return _secondListSelectedPerson; }
            set
            {
                _secondListSelectedPerson = value;
                NotifyOfPropertyChange(() => SecondListSelectedPerson);
            }
        }


        public PersonModel FirstListSelectedPerson
        {
            get { return _firstListSelectedPerson; }
            set
            {
                _firstListSelectedPerson = value;
                NotifyOfPropertyChange(() => FirstListSelectedPerson);
                NotifyOfPropertyChange(() => SecondPersonList);
            }
        }

        public List<PersonModel> SecondPersonList
        {
            get
            {
                if (_firstListSelectedPerson != null)
                    return GetElementsOfSecondPersonList();
                else
                    return _secondPersonList;
            }
            set
            {
                _secondPersonList = value;
                NotifyOfPropertyChange(() => SecondPersonList);
            }
        }

        public List<PersonModel> FirstPersonList
        {
            get
            {
                return _firstPersonList;
            }
            set
            {
                _firstPersonList = value;
                NotifyOfPropertyChange(() => FirstPersonList);
            }
        }
        #region methods
        public List<PersonModel> GetAllPeopleFormDatabase()
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            List<PersonModel> allPeopleInDatabse = new List<PersonModel>();
            var allPeople = db.QueryByExample(new PersonModel());
            foreach (var person in allPeople)
            {
                allPeopleInDatabse.Add((PersonModel)person);
            }
            db.Close();
            return allPeopleInDatabse;
        }
        public List<PersonModel> GetElementsOfSecondPersonList()
        {
            if (_firstListSelectedPerson != null)
                _secondPersonList.Remove(FirstListSelectedPerson);
            return _secondPersonList;
        }
        public void GetCommmonAncestors()
        {
            if(FirstListSelectedPerson.Name == SecondListSelectedPerson.Name)
            {
                MessageBox.Show("Wybrano tę samą osobę", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                CommonAncestorsList = CommonAncestorsHelper.ListOfCommonAncestors(FirstListSelectedPerson.Name, SecondListSelectedPerson.Name);
                NotifyOfPropertyChange(() => CommonAncestorsList);
            }
        }
        #endregion
    }
}
