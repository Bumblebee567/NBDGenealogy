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
            var wrongImported = importedFathers.Select(x => x.Gender == EGender.Female).ToList();
            for (int i = 0; i < wrongImported.Count; i++)
            {
                if (wrongImported[i] == true)
                {
                    importedFathers.RemoveAt(i);
                }
            }
            return importedFathers;
        }
        public static ObservableCollection<PersonModel> RemovePossiblyFathersWithWrongAge(ObservableCollection<PersonModel> possibleFathers)
        {
            List<PersonModel> fathersToRemove = new List<PersonModel>();
            foreach (var person in possibleFathers)
            {
                int differenceInDays = (int)DateTime.Now.Subtract(person.BirthDate).TotalDays;

                if (differenceInDays > 25550 || differenceInDays < 4380)
                {
                    fathersToRemove.Add(person);
                }
            }
            foreach (var fatherToRemove in fathersToRemove)
            {
                possibleFathers.Remove(fatherToRemove);
            }
            return possibleFathers;
        }
    }
}
