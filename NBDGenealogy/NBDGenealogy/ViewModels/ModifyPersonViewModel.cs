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
            get
            {
                if (BirthDate == null)
                    return AllPossibleMothers(_selectedPerson, _selectedPerson.BirthDate.Value);
                else
                    return AllPossibleMothers(_selectedPerson, BirthDate.Value);
            }
            set
            {
                _possibleMothers = value;
                NotifyOfPropertyChange(() => PossibleMothers);
            }
        }
        public BindableCollection<PersonModel> PossibleFathers
        {
            get
            {
                if (BirthDate == null)
                    return AllPossibleFathers(_selectedPerson, _selectedPerson.BirthDate.Value);
                else
                    return AllPossibleFathers(_selectedPerson, BirthDate.Value);
            }
            set
            {
                _possibleFathers = value;
                NotifyOfPropertyChange(() => PossibleFathers);
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
        public PersonModel Mother
        {
            get { return _mother; }
            set
            {
                _mother = value;
                NotifyOfPropertyChange(() => Mother);
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
        public DateTime? DeathDate
        {
            get { return _deathDate; }
            set
            {
                _deathDate = value;
                NotifyOfPropertyChange(() => DeathDate);
            }
        }
        public DateTime? BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                NotifyOfPropertyChange(() => BirthDate);
                NotifyOfPropertyChange(() => PossibleFathers);
                NotifyOfPropertyChange(() => PossibleMothers);
            }
        }
        public PersonModel SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                Name = SelectedPerson.Name;
                BirthDate = SelectedPerson.BirthDate;
                DeathDate = SelectedPerson.DeathDate;
                Gender = SelectedPerson.Gender;
                if (SelectedPerson.Father != null)
                    Father.Name = SelectedPerson.Father;
                if (SelectedPerson.Mother != null)
                    Mother.Name = SelectedPerson.Mother;
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
                if (person.BirthDate != null)
                    person.BirthDate.Value.ToShortDateString();
                if (person.DeathDate != null)
                    person.DeathDate.Value.ToShortDateString();
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
        public BindableCollection<PersonModel> AllPossibleMothers(PersonModel selectedPerson, DateTime birthDate)
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            BindableCollection<PersonModel> possibleMothers = new BindableCollection<PersonModel>();
            var allWomenInDatabase = db.QueryByExample(new PersonModel(EGender.Female));
            foreach (var woman in allWomenInDatabase)
            {
                possibleMothers.Add((PersonModel)woman);
            }
            db.Close();
            possibleMothers = PossibleMothersHelper.RemovePossiblyWrongImportedMothers(possibleMothers) as BindableCollection<PersonModel>;
            if (selectedPerson != null)
            {
                possibleMothers = PossibleMothersHelper.RemovePossiblyMothersWithWrongAge(possibleMothers, birthDate) as BindableCollection<PersonModel>;
            }
            possibleMothers.Add(new PersonModel("-brak-"));
            return possibleMothers;
        }
        public void SaveModifiedPersonToDatabase()
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            var personToModifiy = db.QueryByExample(new PersonModel(SelectedPerson.Name)).Next() as PersonModel;
            var allPeopleInDatabase = db.QueryByExample(new PersonModel());
            List<PersonModel> personHelperList = new List<PersonModel>();
            foreach (var person in allPeopleInDatabase)
            {
                var p = (PersonModel)person;
                personHelperList.Add(p);
            }
            if (Name == null)
            {
                MessageBox.Show("Nie można dodać osoby bez imienia", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                db.Close();
            }
            else if (Name != SelectedPerson.Name && personHelperList.Any(x => x.Name == Name))
            {
                MessageBox.Show("Osoba z podanym imieniem jest już w bazie", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                db.Close();
            }
            else
            {
                personToModifiy.Name = Name;
                personToModifiy.BirthDate = BirthDate;
                personToModifiy.DeathDate = DeathDate;

                if (Gender == EGender.brak)
                    personToModifiy.Gender = null;
                else
                    personToModifiy.Gender = Gender;

                if (Father != null)
                {
                    if (Father != null && Father.Name == "-brak-")
                    {
                        personToModifiy.Father = String.Empty;
                        var previousFather = db.QueryByExample(new PersonModel(SelectedPerson.Father)).Next() as PersonModel;
                        previousFather.Children.Remove(personToModifiy.Name);
                        if (previousFather.Children.Count == 0)
                            previousFather.Children = null;
                        db.Store(previousFather);
                    }
                    else
                    {
                        personToModifiy.Father = Father.Name;
                        var newFather = db.QueryByExample(new PersonModel(Father.Name)).Next() as PersonModel;
                        if (newFather.Children == null)
                            newFather.Children = new List<string>();
                        newFather.Children.Add(Name);
                        db.Store(newFather);
                    }
                }
                if (Mother != null)
                {
                    if (Mother.Name == "-brak-")
                    {
                        personToModifiy.Mother = String.Empty;
                        var previousMother = db.QueryByExample(new PersonModel(SelectedPerson.Mother)).Next() as PersonModel;
                        previousMother.Children.Remove(personToModifiy.Name);
                        if (previousMother.Children.Count == 0)
                            previousMother.Children = null;
                        db.Store(previousMother);
                    }
                    else
                    {
                        personToModifiy.Mother = Mother.Name;
                        var newMother = db.QueryByExample(new PersonModel(Mother.Name)).Next() as PersonModel;
                        if (newMother.Children == null)
                            newMother.Children = new List<string>();
                        newMother.Children.Add(Name);
                        db.Store(newMother);
                    }
                }
            }
            db.Store(personToModifiy);
            db.Close();
            NotifyOfPropertyChange(() => AllPeopleInDatabase);
            Name = null;
            Father = null;
            Mother = null;
            Gender = null;
            BirthDate = null;
            DeathDate = null;
        }
        #endregion
    }
}
