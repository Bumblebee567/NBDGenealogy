using NBDGenealogy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBDGenealogy.Helpers
{
    class PossibleFathersHelper
    {
        public static ObservableCollection<PersonModel> RemovePossiblyWrongImportedFathers(ObservableCollection<PersonModel> importedFathers)
        {
            var wrongImported = importedFathers.Where(x => x.Gender == EGender.Female || x.Gender == null).ToList();
            foreach (var wi in wrongImported)
            {
                importedFathers.Remove(wi);
            }
            return importedFathers;
        }
        public static ObservableCollection<PersonModel> RemovePossiblyFathersWithWrongAge(ObservableCollection<PersonModel> possibleFathers, DateTime childBirthDate)
        {
            const int twelveYearsInTotalDays = 4380;
            const int seventyYearsInTotalDays = 25550;
            List<PersonModel> fathersToRemove = new List<PersonModel>();
            foreach (var person in possibleFathers)
            {
                if (person.BirthDate != null)
                {
                    int differenceInDays = (int)childBirthDate.Subtract(person.BirthDate.Value).TotalDays;

                    if (differenceInDays > seventyYearsInTotalDays || differenceInDays < twelveYearsInTotalDays)
                    {
                        fathersToRemove.Add(person);
                    }
                }
            }
            foreach (var fatherToRemove in fathersToRemove)
            {
                possibleFathers.Remove(fatherToRemove);
            }
            fathersToRemove.Clear();

            foreach (var person in possibleFathers)
            {
                if(person.DeathDate != null)
                {
                    int differenceBetweenFathersDeathAndChildBirth = (int)childBirthDate.Subtract(person.DeathDate.Value).TotalDays;

                    if(differenceBetweenFathersDeathAndChildBirth > 270)
                    {
                        fathersToRemove.Add(person);
                    }
                }
            }
            foreach (var fatherToRemove in fathersToRemove)
            {
                possibleFathers.Remove(fatherToRemove);
            }
            return possibleFathers;
        }
        public static ObservableCollection<PersonModel> RemoveDescendantsFromPossibleFathers(ObservableCollection<PersonModel> possibleFathers, PersonModel person)
        {
            var personsDescendants = DescendantsHelper.GetPersonDescendants(person);
            foreach (var descendant in personsDescendants)
            {
                var item = possibleFathers.Where(x => x.Name == descendant.Name).First();
                possibleFathers.Remove(item);
            }
            return possibleFathers;
        }
        public static bool IsOriginalFatherCorrectAfterBirthDateModification(DateTime newBirthDate, DateTime selectedPersonFatherBirthDate)
        {
            const int twelveYearsInTotalDays = 4380;
            const int seventyYearsInTotalDays = 25550;
            int differenceInDays = (int)newBirthDate.Subtract(selectedPersonFatherBirthDate).TotalDays;
            if (differenceInDays > seventyYearsInTotalDays || differenceInDays < twelveYearsInTotalDays)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
