using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction.Configuration
{
    public enum NdeSearchConditionComparison
    {
        Equal = 0,
        HasPropertyByDisplayName = 1,
        HasPropertyByDisplayNameAndValue = 2,
        Other
    }
}