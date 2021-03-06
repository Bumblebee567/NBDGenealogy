﻿using NBDGenealogy.Models;
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
                if (person.DeathDate != null)
                {
                    int differenceInDays = (int)childBirthDate.Subtract(person.DeathDate.Value).TotalDays;

                    if (differenceInDays > 0)
                    {
                        mothersToRemove.Add(person);
                        continue;
                    }
                }
                if (person.BirthDate != null)
                {
                    int differenceInDays = (int)childBirthDate.Subtract(person.BirthDate.Value).TotalDays;

                    if (differenceInDays > sixtyYearsInTotalDays || differenceInDays < tenYearsInTotalDays)
                    {
                        mothersToRemove.Add(person);
                    }
                }
            }
            foreach (var motherToRemove in mothersToRemove)
            {
                possibleMothers.Remove(motherToRemove);
            }
            return possibleMothers;
        }
        public static ObservableCollection<PersonModel> RemoveDescendantsFromPossibleMothers(ObservableCollection<PersonModel> possibleMothers, PersonModel person)
        {
            var personsDescendants = DescendantsHelper.GetPersonDescendants(person);
            var items = possibleMothers.Where(x => x.Name == person.Name);
            if (items != null)
            {
                foreach (var thisPerson in items.ToList())
                {
                    possibleMothers.Remove(thisPerson);
                }
            }
            foreach (var descendant in personsDescendants.ToList())
            {
                var d = possibleMothers.Where(x => x.Name == descendant.Name).FirstOrDefault();
                if(d != null)
                    possibleMothers.Remove(d);
            }
            return possibleMothers;
        }
    }
}
