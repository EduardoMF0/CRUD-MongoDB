using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_MongoDB
{
        public static class StringExtensions
        {
            public static string? FirstChar(this string input) =>
                string.IsNullOrEmpty(input)
                    ? null
                    : string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1));
        }
}
