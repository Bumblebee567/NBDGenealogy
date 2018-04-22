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
            //var peopleWithIncorrectAge = possibleFathers.Select(x => DateTime.Now.Subtract(x.BirthDate).TotalDays >= 4380 && x.BirthDate.AddYears(-70) <= DateTime.MinValue).ToList();
            foreach (var person in possibleFathers)
            {
                if(DateTime.Now.Subtract(person.BirthDate).TotalDays < 4380)
                {
                    if(DateTime.Now.Subtract(person.BirthDate).TotalDays > 25550)
                    {
                        possibleFathers.Remove(person);
                    }
                }
            }
            //for (int i = 0; i < peopleWithIncorrectAge.Count; i++)
            //{
            //    if(peopleWithIncorrectAge[i] == false)
            //    {
            //        possibleFathers.RemoveAt(i);
            //    }
            //}
            return possibleFathers;
        }
    }
}
