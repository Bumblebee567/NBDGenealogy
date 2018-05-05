using Caliburn.Micro;
using Db4objects.Db4o;
using NBDGenealogy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBDGenealogy.ViewModels
{
    class CommonAncestorsViewModel : Screen
    {
        public CommonAncestorsViewModel()
        {
            FirstPersonList = GetAllPeopleFormDatabase();
            SecondPersonList = GetAllPeopleFormDatabase();
        }
        private List<PersonModel> _firstPersonList;
        private List<PersonModel> _secondPersonList;
        private PersonModel _firstListSelectedPerson;
        private PersonModel _secondListSelectedPerson;

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
                _secondPersonList.Remove(_firstListSelectedPerson);
                NotifyOfPropertyChange(() => FirstListSelectedPerson);
                NotifyOfPropertyChange(() => SecondPersonList);
            }
        }

        public List<PersonModel> SecondPersonList
        {
            get { return _secondPersonList; }
            set
            {
                _secondPersonList = value;
                NotifyOfPropertyChange(() => SecondPersonList);
            }
        }

        public List<PersonModel> FirstPersonList
        {
            get { return _firstPersonList; }
            set
            {
                _firstPersonList = value;
                NotifyOfPropertyChange(() => FirstPersonList);
            }
        }

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
    }
}
