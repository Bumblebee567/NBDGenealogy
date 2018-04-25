using NBDGenealogy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBDGenealogy.Helpers
{
    class PossibleMothersHelper
    {
        public static ObservableCollection<PersonModel> RemovePossiblyWrongImportedMothers(ObservableCollection<PersonModel> importedMothers)
        {
            var wrongImported = importedMothers.Where(x => x.Gender == EGender.Male).ToList();
            foreach (var wi in wrongImported)
            {
                importedMothers.Remove(wi);
            }
            return importedMothers;
        }
        public static ObservableCollection<PersonModel> RemovePossiblyMothersWithWrongAge(ObservableCollection<PersonModel> possibleMothers, DateTime childBirthDate)
        {
            const int tenYearsInTotalDays = 3650;
            const int sixtyYearsInTotalDays = 21900;
            List<PersonModel> mothersToRemove = new List<PersonModel>();
            foreach (var person in possibleMothers)
            {
                int differenceInDays = (int)childBirthDate.Subtract(person.BirthDate).TotalDays;

                if (differenceInDays > sixtyYearsInTotalDays || differenceInDays < tenYearsInTotalDays)
                {
                    mothersToRemove.Add(person);
                }
            }
            foreach (var motherToRemove in mothersToRemove)
            {
                possibleMothers.Remove(motherToRemove);
            }
            return possibleMothers;
        }
    }
}
