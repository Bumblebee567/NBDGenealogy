﻿using Caliburn.Micro;
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
                if (_selectedPerson.BirthDate == null)
                    return AllPossibleMothers(_selectedPerson);
                else if (BirthDate == null)
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
                if (_selectedPerson.BirthDate == null)
                    return AllPossibleFathers(_selectedPerson);
                else if (BirthDate == null)
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
                NotifyOfPropertyChange(() => SelectedPerson);
                Name = SelectedPerson.Name;
                BirthDate = SelectedPerson.BirthDate;
                DeathDate = SelectedPerson.DeathDate;
                Gender = SelectedPerson.Gender;
                if (SelectedPerson.Father != null)
                    Father.Name = SelectedPerson.Father;
                if (SelectedPerson.Mother != null)
                    Mother.Name = SelectedPerson.Mother;
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
                possibleFathers = PossibleFathersHelper.RemovePossiblyFathersWithWrongAge(possibleFathers, birthDate) as BindableCollection<PersonModel>;
                possibleFathers = PossibleFathersHelper.RemoveDescendantsFromPossibleFathers(possibleFathers, selectedPerson) as BindableCollection<PersonModel>;
            }
            possibleFathers.Add(new PersonModel("-brak-"));
            return possibleFathers;
        }
        public BindableCollection<PersonModel> AllPossibleFathers(PersonModel person)
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            BindableCollection<PersonModel> possibleFathers = new BindableCollection<PersonModel>();
            var allMenInDatabase = db.QueryByExample(new PersonModel(EGender.Male));
            foreach (var man in allMenInDatabase)
            {
                possibleFathers.Add((PersonModel)man);
            }
            db.Close();
            List<PersonModel> menWithBirthDate = new List<PersonModel>();
            foreach (var man in possibleFathers)
            {
                if (man.BirthDate != null)
                    menWithBirthDate.Add(man);
            }
            foreach (var man in menWithBirthDate)
            {
                possibleFathers.Remove(man);
            }
            possibleFathers = PossibleFathersHelper.RemovePossiblyWrongImportedFathers(possibleFathers) as BindableCollection<PersonModel>;
            possibleFathers = PossibleFathersHelper.RemoveDescendantsFromPossibleFathers(possibleFathers, person) as BindableCollection<PersonModel>;
            var items = possibleFathers.Where(x => x.Name == person.Name);
            if (items != null)
            {
                foreach (var thisPerson in items.ToList())
                {
                    possibleFathers.Remove(thisPerson);
                }
            }
            items = null;
            possibleFathers.Add(new PersonModel("-brak-"));
            return possibleFathers;
        }
        public BindableCollection<PersonModel> AllPossibleMothers(PersonModel person)
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            BindableCollection<PersonModel> possibleMothers = new BindableCollection<PersonModel>();
            var allWomenInDatabase = db.QueryByExample(new PersonModel(EGender.Female));
            foreach (var man in allWomenInDatabase)
            {
                possibleMothers.Add((PersonModel)man);
            }
            db.Close();
            List<PersonModel> womenWithBirthDate = new List<PersonModel>();
            foreach (var woman in possibleMothers)
            {
                if (woman.BirthDate != null)
                    womenWithBirthDate.Add(woman);
            }
            foreach (var woman in womenWithBirthDate)
            {
                possibleMothers.Remove(woman);
            }
            possibleMothers = PossibleMothersHelper.RemovePossiblyWrongImportedMothers(possibleMothers) as BindableCollection<PersonModel>;
            possibleMothers = PossibleMothersHelper.RemoveDescendantsFromPossibleMothers(possibleMothers, person) as BindableCollection<PersonModel>;
            var items = possibleMothers.Where(x => x.Name == person.Name);
            if (items != null)
            {
                foreach (var thisPerson in items.ToList())
                {
                    possibleMothers.Remove(thisPerson);
                }
            }
            possibleMothers.Add(new PersonModel("-brak-"));
            return possibleMothers;
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
                possibleMothers = PossibleMothersHelper.RemoveDescendantsFromPossibleMothers(possibleMothers, selectedPerson) as BindableCollection<PersonModel>;
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
                if(Name != SelectedPerson.Name)
                {
                    if(personToModifiy.Father != String.Empty && personToModifiy.Father != null)
                    {
                        var father = db.QueryByExample(new PersonModel(personToModifiy.Father)).Next() as PersonModel;
                        var child = father.Children.Where(x => x == SelectedPerson.Name).FirstOrDefault();
                        var newNameChild = Name;
                        int index = father.Children.IndexOf(child);
                        if (index != -1)
                            father.Children[index] = newNameChild;
                        db.Store(father.Children);
                    }
                    if(personToModifiy.Mother != String.Empty && personToModifiy.Mother != null)
                    {
                        var mother = db.QueryByExample(new PersonModel(personToModifiy.Mother)).Next() as PersonModel;
                        var child = mother.Children.Where(x => x == SelectedPerson.Name).FirstOrDefault();
                        var newNameChild = Name;
                        int index = mother.Children.IndexOf(child);
                        if (index != -1)
                            mother.Children[index] = newNameChild;
                        db.Store(mother.Children);
                    }
                }

                personToModifiy.BirthDate = BirthDate;

                if (DeathDate != null)
                {
                    bool isDeathDatePossible = IsDeathDatePossible(DeathDate.Value, personToModifiy, db);
                    if (isDeathDatePossible == true)
                    {
                        personToModifiy.DeathDate = DeathDate;
                    }
                    else
                    {
                        db.Close();
                        MessageBox.Show("Nie można ustawić podanej daty śmierci - osoba posiada dzieci urodzone później"
                            , "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                if (Gender == EGender.brak)
                {
                    personToModifiy.Gender = null;

                    if (SelectedPerson.Gender == EGender.Male)
                    {
                        var personsChildren = db.QueryByExample(PersonModel.WithFather(Name));
                        List<PersonModel> children = new List<PersonModel>();
                        foreach (var child in personsChildren)
                        {
                            children.Add(child as PersonModel);
                        }
                        foreach (var child in children)
                        {
                            child.Father = String.Empty;
                        }
                        foreach (var child in personsChildren)
                        {
                            db.Store(child);
                        }
                        if (personToModifiy.Children != null)
                        {
                            personToModifiy.Children = null;
                        }
                    }
                    else if (SelectedPerson.Gender == EGender.Female)
                    {
                        var personsChildren = db.QueryByExample(PersonModel.WithMother(Name));
                        List<PersonModel> children = new List<PersonModel>();
                        foreach (var child in personsChildren)
                        {
                            children.Add(child as PersonModel);
                        }
                        foreach (var child in children)
                        {
                            child.Mother = String.Empty;
                        }
                        foreach (var child in personsChildren)
                        {
                            db.Store(child);
                        }
                        if (personToModifiy.Children != null)
                        {
                            personToModifiy.Children = null;
                        }
                    }
                }
                else
                {
                    personToModifiy.Gender = Gender;
                    if (SelectedPerson.Gender != Gender)
                    {
                        if (personToModifiy.Children != null)
                        {
                            personToModifiy.Children = null;
                            if (Gender == EGender.Male && SelectedPerson.Gender == EGender.Female)
                            {
                                var personsChildren = db.QueryByExample(PersonModel.WithMother(Name));
                                List<PersonModel> children = new List<PersonModel>();
                                foreach (var child in personsChildren)
                                {
                                    children.Add(child as PersonModel);
                                }
                                foreach (var child in children)
                                {
                                    child.Mother = String.Empty;
                                }
                                foreach (var child in personsChildren)
                                {
                                    db.Store(child);
                                }
                            }
                            else if (Gender == EGender.Female && SelectedPerson.Gender == EGender.Male)
                            {
                                var personsChildren = db.QueryByExample(PersonModel.WithFather(Name));
                                List<PersonModel> children = new List<PersonModel>();
                                foreach (var child in personsChildren)
                                {
                                    children.Add(child as PersonModel);
                                }
                                foreach (var child in children)
                                {
                                    child.Father = String.Empty;
                                }
                                foreach (var child in personsChildren)
                                {
                                    db.Store(child);
                                }
                            }
                        }
                    }
                }

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
                        {
                            newFather.Children = new List<string>();
                            newFather.Children.Add(Name);
                            db.Store(newFather);
                        }
                        else
                        {
                            newFather.Children.Add(Name);
                            db.Store(newFather.Children);
                        }
                        if (SelectedPerson.Father != null && SelectedPerson.Father != String.Empty)
                        {
                            var previousFather = db.QueryByExample(new PersonModel(SelectedPerson.Father)).Next() as PersonModel;
                            previousFather.Children.Remove(personToModifiy.Name);
                            if (previousFather.Children.Count == 0)
                                previousFather.Children = null;
                            db.Store(previousFather);
                        }
                    }
                }
                else
                {
                    if (SelectedPerson.Father != String.Empty && SelectedPerson.BirthDate != BirthDate)
                    {
                        MessageBox.Show("Ojciec musi zostać wybrany", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        db.Close();
                        return;
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
                        {
                            newMother.Children = new List<string>();
                            newMother.Children.Add(Name);
                            db.Store(newMother);
                        }
                        else
                        {
                            newMother.Children.Add(Name);
                            db.Store(newMother.Children);
                        }
                        if (SelectedPerson.Mother != null && SelectedPerson.Mother != String.Empty)
                        {
                            var previousMother = db.QueryByExample(new PersonModel(SelectedPerson.Mother)).Next() as PersonModel;
                            previousMother.Children.Remove(personToModifiy.Name);
                            if (previousMother.Children.Count == 0)
                                previousMother.Children = null;
                            db.Store(previousMother);
                        }
                    }
                }
                else
                {
                    if (SelectedPerson.Mother != String.Empty && SelectedPerson.BirthDate != BirthDate)
                    {
                        MessageBox.Show("Matka musi zostać wybrana", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        db.Close();
                        return;
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
        public bool IsDeathDatePossible(DateTime deathDate, PersonModel person, IObjectContainer db)
        {
            List<PersonModel> personChildren = new List<PersonModel>();
            if (person.Children != null)
            {
                foreach (var child in person.Children)
                {
                    var p = db.QueryByExample(new PersonModel(child)).Next() as PersonModel;
                    personChildren.Add(p);
                }
                if (personChildren.Any(x => x.BirthDate > deathDate))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
