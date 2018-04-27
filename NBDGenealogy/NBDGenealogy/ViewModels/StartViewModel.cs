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

namespace NBDGenealogy.ViewModels
{
    public class StartViewModel : Conductor<object>
    {
        private string _name;
        private DateTime _birthDate = DateTime.MinValue;
        private DateTime _deathDate = DateTime.MinValue;
        private EGender? _gender = null;
        private PersonModel _father;
        private PersonModel _mother;
        private List<string> _children;
        private ObservableCollection<PersonModel> _possibleFathers;
        private ObservableCollection<PersonModel> _possibleMothers;
        private BindableCollection<PersonModel> _allPeopleInDatabase;
        private PersonModel _selectedPerson;
        private ObservableCollection<PersonModel> _modifiedPersonPossibleFathers;
        private ObservableCollection<PersonModel> _mofifiedPersonPossibleMother;
        private DateTime _selectedPersonBirthDate;
        private PersonModel _selectedPersonFather;
        private PersonModel _selectedPersonMother;
        public string SelectedName { get; set; }
        public DateTime ModifiedPersonOriginalBirthDate { get; set; }

        public PersonModel SelectedPersonMother
        {
            get { return _selectedPersonMother; }
            set
            {
                _selectedPersonMother = value;
                NotifyOfPropertyChange(() => SelectedPersonMother);
            }
        }


        public PersonModel SelectedPersonFather
        {
            get { return _selectedPersonFather; }
            set
            {
                _selectedPersonFather = value;
                NotifyOfPropertyChange(() => SelectedPersonFather);
            }
        }


        public DateTime SelectedPersonBirthDate
        {
            get { return _selectedPersonBirthDate; }
            set
            {
                _selectedPersonBirthDate = SelectedPerson.BirthDate;
                AllPossibleFathers(SelectedPerson, _birthDate);
                AllPossibleMothers(SelectedPerson, _birthDate);
                NotifyOfPropertyChange(() => SelectedPerson);
                NotifyOfPropertyChange(() => SelectedPersonBirthDate);
            }
        }


        public ObservableCollection<PersonModel> ModifiedPersonPossibleMothers
        {
            get
            {
                return AllPossibleMothers(_selectedPerson, _birthDate);
            }
            set
            {
                _mofifiedPersonPossibleMother = value;
                NotifyOfPropertyChange(() => ModifiedPersonPossibleMothers);
            }
        }


        public ObservableCollection<PersonModel> ModifiedPersonPossibleFathers
        {
            get
            {
                return AllPossibleFathers(_selectedPerson, _birthDate);
            }
            set
            {
                _modifiedPersonPossibleFathers = value;
                NotifyOfPropertyChange(() => ModifiedPersonPossibleFathers);
            }
        }

        public PersonModel SelectedPerson
        {
            get
            {
                return _selectedPerson;
            }
            set
            {
                _selectedPerson = value;
                if (SelectedName == null)
                {
                    SelectedName = value.Name;
                    ModifiedPersonOriginalBirthDate = value.BirthDate;
                    BirthDate = _selectedPerson.BirthDate;
                }
                NotifyOfPropertyChange(() => SelectedPerson);
                _modifiedPersonPossibleFathers = AllPossibleFathers(_selectedPerson, _birthDate);
                _mofifiedPersonPossibleMother = AllPossibleMothers(_selectedPerson, _birthDate);
                NotifyOfPropertyChange(() => ModifiedPersonPossibleFathers);
                NotifyOfPropertyChange(() => ModifiedPersonPossibleMothers);
                NotifyOfPropertyChange(() => SelectedPersonBirthDate);
            }
        }

        public BindableCollection<PersonModel> AllPeopleInDatabase
        {
            get { return GetAllPersonsFormDatabase(); }
            set
            {
                _allPeopleInDatabase = value;
                NotifyOfPropertyChange(() => AllPeopleInDatabase);
            }
        }


        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }
        public DateTime BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                NotifyOfPropertyChange(() => BirthDate);
                AllPossibleFathers();
                ModifiedPersonPossibleFathers = AllPossibleFathers(_selectedPerson, _birthDate);
                ModifiedPersonPossibleMothers = AllPossibleMothers(_selectedPerson, _birthDate);
                NotifyOfPropertyChange(() => PossibleFathers);
                NotifyOfPropertyChange(() => PossibleMothers);
            }
        }
        public DateTime DeathDate
        {
            get { return _deathDate; }
            set
            {
                _deathDate = value;
                NotifyOfPropertyChange(() => DeathDate);
            }
        }
        public EGender? Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                NotifyOfPropertyChange(() => Gender);
            }
        }
        public PersonModel Father
        {
            get { return _father; }
            set
            {
                _father = value;
                NotifyOfPropertyChange(() => Father);
            }
        }
        public PersonModel Mother
        {
            get { return _mother; }
            set
            {
                _mother = value;
                NotifyOfPropertyChange(() => Mother);
            }
        }
        public List<string> Children
        {
            get { return _children; }
            set { _children = value; }
        }
        public ObservableCollection<PersonModel> PossibleFathers
        {
            get { return AllPossibleFathers(); }
            set
            {
                _possibleFathers = value;
                NotifyOfPropertyChange(() => PossibleFathers);
            }
        }
        public ObservableCollection<PersonModel> PossibleMothers
        {
            get { return AllPossibleMothers(); }
            set
            {
                _possibleMothers = value;
                NotifyOfPropertyChange(() => PossibleMothers);
            }
        }
        public IList<EGender> GenderTypes
        {
            get
            {
                return Enum.GetValues(typeof(EGender)).Cast<EGender>().ToList<EGender>();
            }
        }
        public BindableCollection<PersonModel> GetAllPersonsFormDatabase()
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
        public ObservableCollection<PersonModel> AllPossibleFathers()
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            ObservableCollection<PersonModel> possibleFathers = new ObservableCollection<PersonModel>();
            var allMenInDatabase = db.QueryByExample(new PersonModel(EGender.Male));
            foreach (var man in allMenInDatabase)
            {
                possibleFathers.Add((PersonModel)man);
            }
            db.Close();
            possibleFathers = PossibleFathersHelper.RemovePossiblyWrongImportedFathers(possibleFathers);
            if (BirthDate != DateTime.MinValue)
                possibleFathers = PossibleFathersHelper.RemovePossiblyFathersWithWrongAge(possibleFathers, BirthDate);
            return possibleFathers;
        }
        public ObservableCollection<PersonModel> AllPossibleMothers()
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            ObservableCollection<PersonModel> possibleMothers = new ObservableCollection<PersonModel>();
            var allWomenInDatabase = db.QueryByExample(new PersonModel(EGender.Female));
            foreach (var woman in allWomenInDatabase)
            {
                possibleMothers.Add((PersonModel)woman);
            }
            db.Close();
            possibleMothers = PossibleMothersHelper.RemovePossiblyWrongImportedMothers(possibleMothers);
            if (BirthDate != DateTime.MinValue)
                possibleMothers = PossibleMothersHelper.RemovePossiblyMothersWithWrongAge(possibleMothers, BirthDate);
            return possibleMothers;
        }
        public ObservableCollection<PersonModel> AllPossibleFathers(PersonModel selectedPerson, DateTime birthDate)
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            ObservableCollection<PersonModel> possibleFathers = new ObservableCollection<PersonModel>();
            var allMenInDatabase = db.QueryByExample(new PersonModel(EGender.Male));
            foreach (var man in allMenInDatabase)
            {
                possibleFathers.Add((PersonModel)man);
            }
            db.Close();
            possibleFathers = PossibleFathersHelper.RemovePossiblyWrongImportedFathers(possibleFathers);
            if (selectedPerson != null)
            {
                if (selectedPerson.BirthDate != DateTime.MinValue)
                    possibleFathers = PossibleFathersHelper.RemovePossiblyFathersWithWrongAge(possibleFathers, birthDate);
            }
            possibleFathers.Add(new PersonModel("-brak-"));
            return possibleFathers;
        }
        public ObservableCollection<PersonModel> AllPossibleMothers(PersonModel selectedPerson, DateTime birthDate)
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            ObservableCollection<PersonModel> possibleMothers = new ObservableCollection<PersonModel>();
            var allWomenInDatabase = db.QueryByExample(new PersonModel(EGender.Female));
            foreach (var woman in allWomenInDatabase)
            {
                possibleMothers.Add((PersonModel)woman);
            }
            db.Close();
            possibleMothers = PossibleMothersHelper.RemovePossiblyWrongImportedMothers(possibleMothers);
            if (selectedPerson != null)
            {
                if (selectedPerson.BirthDate != DateTime.MinValue)
                    possibleMothers = PossibleMothersHelper.RemovePossiblyMothersWithWrongAge(possibleMothers, birthDate);
            }
            possibleMothers.Add(new PersonModel("-brak-"));
            return possibleMothers;
        }
        public void AddPersonToDatabase()
        {
            if (Name == null)
            {
                MessageBox.Show("Nie można dodać osoby bez imienia", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                IObjectContainer db = Db4oFactory.OpenFile("person.data");
                var allPeopleInDatabase = db.QueryByExample(new PersonModel());
                List<PersonModel> personHelperList = new List<PersonModel>();
                foreach (var person in allPeopleInDatabase)
                {
                    var p = (PersonModel)person;
                    personHelperList.Add(p);
                }
                if (personHelperList.Any(x => x.Name == Name))
                {
                    MessageBox.Show("Osoba o podanym imieniu jest już w bazie", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    db.Close();
                    Name = null;
                    return;
                }
                else
                {
                    PersonModel newPerson = new PersonModel
                    {
                        Name = Name,
                        BirthDate = BirthDate,
                        DeathDate = DeathDate,
                        Gender = Gender
                    };
                    if (Father != null)
                        newPerson.Father = Father.Name;
                    if (Mother != null)
                        newPerson.Mother = Mother.Name;
                    if (newPerson.Father != null)
                    {
                        var newPersonFather = (PersonModel)db.QueryByExample(new PersonModel(newPerson.Father)).Next();
                        if (newPersonFather.Children == null)
                        {
                            newPersonFather.Children = new List<string>();
                        }
                        newPersonFather.Children.Add(newPerson.Name);
                        db.Store(newPersonFather);
                    }
                    if (newPerson.Mother != null)
                    {
                        var newPersonMother = (PersonModel)db.QueryByExample(new PersonModel(newPerson.Mother)).Next();
                        if (newPersonMother.Children == null)
                        {
                            newPersonMother.Children = new List<string>();
                        }
                        newPersonMother.Children.Add(newPerson.Name);
                        db.Store(newPersonMother);
                    }
                    db.Store(newPerson);
                    db.Close();
                    Name = null;
                    Father = null;
                    Mother = null;
                    BirthDate = DateTime.MinValue;
                    DeathDate = DateTime.MinValue;
                    Gender = null;
                    NotifyOfPropertyChange(nameof(PossibleFathers));
                    NotifyOfPropertyChange(nameof(PossibleMothers));
                    NotifyOfPropertyChange(nameof(AllPeopleInDatabase));
                }

            }
        }
        public void SaveModifiedPerson()
        {
            if (SelectedPerson.Name == null)
            {
                MessageBox.Show("Nie można dodać osoby bez imienia", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                IObjectContainer db = Db4oFactory.OpenFile("person.data");
                var allPeopleInDatabase = db.QueryByExample(new PersonModel());
                List<PersonModel> personHelperList = new List<PersonModel>();
                foreach (var person in allPeopleInDatabase)
                {
                    var p = (PersonModel)person;
                    personHelperList.Add(p);
                }

                var personToModify = db.QueryByExample(new PersonModel(SelectedName)).Next() as PersonModel;
                personToModify.Name = SelectedPerson.Name;
                if (SelectedPersonFather != null)
                    personToModify.Father = SelectedPersonFather.Name;
                if (SelectedPersonMother != null)
                    personToModify.Mother = SelectedPersonMother.Name;
                personToModify.BirthDate = BirthDate;
                personToModify.DeathDate = SelectedPerson.DeathDate;
                personToModify.Gender = SelectedPerson.Gender;
                if (SelectedPersonFather.Name != "-brak-")
                    personToModify.Father = SelectedPersonFather.Name;
                else
                {
                    var newPersonFather = (PersonModel)db.QueryByExample(new PersonModel(SelectedPerson.Father)).Next();
                    newPersonFather.Children.Remove(SelectedPerson.Name);
                    if (newPersonFather.Children.Count == 0)
                    {
                        newPersonFather.Children = null;
                    }
                    db.Store(newPersonFather);
                    personToModify.Father = null;
                }

                if (SelectedPersonMother.Name != "-brak-")
                    personToModify.Mother = SelectedPersonMother.Name;
                else
                {
                    var newPersonMother = (PersonModel)db.QueryByExample(new PersonModel(SelectedPerson.Mother)).Next();
                    newPersonMother.Children.Remove(SelectedPerson.Name);
                    if (newPersonMother.Children.Count == 0)
                    {
                        newPersonMother.Children = null;
                    }
                    db.Store(newPersonMother);
                    personToModify.Mother = null;
                }
                if (SelectedPersonFather.Name != "-brak")
                {
                    var newPersonFather = (PersonModel)db.QueryByExample(new PersonModel(personToModify.Father)).Next();
                    if (newPersonFather.Children == null)
                    {
                        newPersonFather.Children = new List<string>();
                    }
                    newPersonFather.Children.Add(personToModify.Name);
                    db.Store(newPersonFather);
                }
                if (SelectedPersonMother.Name != "-brak")
                {
                    var newPersonMother = (PersonModel)db.QueryByExample(new PersonModel(personToModify.Mother)).Next();
                    if (newPersonMother.Children == null)
                    {
                        newPersonMother.Children = new List<string>();
                    }
                    newPersonMother.Children.Add(personToModify.Name);
                    db.Store(newPersonMother);
                }
                db.Store(personToModify);
                db.Close();
                SelectedPerson = null;
                NotifyOfPropertyChange(nameof(AllPeopleInDatabase));

            }
        }
        public void LoadModifyPersonView()
        {
            ActivateItem(new ModifyPersonViewModel());
        }
    }
}
