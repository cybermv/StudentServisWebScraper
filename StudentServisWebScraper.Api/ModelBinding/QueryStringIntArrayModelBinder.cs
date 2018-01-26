using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using Microsoft.Extensions.Primitives;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace StudentServisWebScraper.Api.ModelBinding
{
    public class QueryStringIntArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            if (bindingContext.ModelType != typeof(int[]))
            {
                throw new ArgumentException($"Cannot use '{nameof(QueryStringIntArrayModelBinder)}' with a non int[] parameter.");
            }

            StringValues queryStringArray = bindingContext.HttpContext.Request.Query[bindingContext.ModelName];

            List<int> jointArr = new List<int>();

            foreach (string item in queryStringArray)
            {
                int[] arr = ParseValues(item);
                jointArr.AddRange(arr);
            }

            bindingContext.Result = ModelBindingResult.Success(jointArr.ToArray());

            return Task.CompletedTask;
        }

        private int[] ParseValues(string arrayExpression)
        {
            arrayExpression = arrayExpression.Replace(" ", "");

            Match m = Regex.Match(arrayExpression, @"\[((\d+)\,?)*\]");

            if (!m.Success)
            {
                throw new ArgumentException("Malformed parameter!");
            }

            MatchCollection ms = Regex.Matches(m.Value, @"\d+");

            int[] arr = new int[ms.Count];

            for (int i = 0; i < ms.Count; i++)
            {
                arr[i] = int.Parse(ms[i].Value);
            }

            return arr;
        }
    }
}
