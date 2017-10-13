using System.Collections.Generic;
using System.Linq;
using ConjuctorAPI.Models;

namespace ConjuctorAPI.Utils
{
    public static class ConjuctionUtils
    {
        public static List<string> GetFormsFromConjuction(Conjuction conjuction, string method) {
            var forms = new List<string>();
            foreach (var tense in conjuction.Tenses.Values)
                foreach (var formm in tense.Forms)
                    if (method == "word") 
                    {
                        if (!formm.Form.Contains(" "))
                            forms.Add(formm.Form);
                    }
                    else 
                        forms.Add(formm.Form);
            
            forms = forms.Distinct().ToList();
            return forms;
        }
    }
}