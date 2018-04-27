using Caliburn.Micro;
using Db4objects.Db4o;
using NBDGenealogy.Helpers;
using NBDGenealogy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBDGenealogy.ViewModels
{
    class ModifyPersonViewModel : Screen
    {
        private BindableCollection<PersonModel> _allPeopleInDatabase;
        private PersonModel _selectedPerson;
        private string _name;
        private DateTime? _birthDate;
        private DateTime? _deathDate;
        private PersonModel _father;
        private PersonModel _mother;
        private EGender? _gender;
        private BindableCollection<PersonModel> _possibleFathers;
        private BindableCollection<PersonModel> _possibleMothers;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public BindableCollection<PersonModel> PossibleMothers
        {
            get { return _possibleMothers; }
            set
            {
                _possibleMothers = value;
                NotifyOfPropertyChange(() => PossibleMothers);
            }
        }
        public BindableCollection<PersonModel> PossibleFathers
        {
            get { return AllPossibleFathers(_selectedPerson, _birthDate.Value); }
            set
            {
                _possibleFathers = value;
                NotifyOfPropertyChange(() => PossibleFathers);
            }
        }
        public EGender? Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }
        public PersonModel Mother
        {
            get { return _mother; }
            set { _mother = value; }
        }
        public PersonModel Father
        {
            get { return _father; }
            set { _father = value; }
        }
        public DateTime? DeathDate
        {
            get { return _deathDate; }
            set { _deathDate = value; }
        }
        public DateTime? BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                NotifyOfPropertyChange(() => BirthDate);
                NotifyOfPropertyChange(() => PossibleMothers);
                NotifyOfPropertyChange(() => PossibleFathers);
            }
        }
        public PersonModel SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                _name = SelectedPerson.Name;
                _birthDate = SelectedPerson.BirthDate;
                _deathDate = SelectedPerson.DeathDate;
                _gender = SelectedPerson.Gender;
                _father.Name = SelectedPerson.Father;
                _mother.Name = SelectedPerson.Mother;
                NotifyOfPropertyChange(() => SelectedPerson);
            }
        }
        public BindableCollection<PersonModel> AllPeopleInDatabase
        {
            get
            {
                return GetAllPeopleFormDatabase();
            }
            set
            {
                _allPeopleInDatabase = value;
                NotifyOfPropertyChange(() => AllPeopleInDatabase);
            }
        }
        public IList<EGender> GenderTypes
        {
            get
            {
                return Enum.GetValues(typeof(EGender)).Cast<EGender>().ToList<EGender>();
            }
        }

        #region Methods
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
                person.BirthDate.ToShortDateString();
                person.DeathDate.ToShortDateString();
            }
            return allPeopleInDatabse;
        }
        public BindableCollection<PersonModel> AllPossibleFathers(PersonModel selectedPerson, DateTime birthDate)
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            BindableCollection<PersonModel> possibleFathers = new BindableCollection<PersonModel>();
            var allMenInDatabase = db.QueryByExample(new PersonModel(EGender.Male));
            foreach (var man in allMenInDatabase)
            {
                possibleFathers.Add((PersonModel)man);
            }
            db.Close();
            possibleFathers = PossibleFathersHelper.RemovePossiblyWrongImportedFathers(possibleFathers) as BindableCollection<PersonModel>;
            if (selectedPerson != null)
            {
                if (selectedPerson.BirthDate != DateTime.MinValue)
                    possibleFathers = PossibleFathersHelper.RemovePossiblyFathersWithWrongAge(possibleFathers, birthDate) as BindableCollection<PersonModel>;
            }
            possibleFathers.Add(new PersonModel("-brak-"));
            return possibleFathers;
        }
        #endregion
    }
}
