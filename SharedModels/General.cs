using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels {
    public static class General {
        public static string GetDescription(string description) {
            int maxChars = 100;
            return description.Length > maxChars ? $"{description.Substring(0, maxChars)}..." : description;
        }
    }
}
