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
            var wrongImported = importedFathers.Select(x => x.Gender == EGender.Female).ToArray();
            for (int i = 0; i < wrongImported.Count(); i++)
            {
                if (wrongImported[i] == true)
                {
                    importedFathers.RemoveAt(i);
                }
            }
            return importedFathers;
        }
    }
}
