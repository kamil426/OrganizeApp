using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Shared.Common.Enums
{
    public enum TaskStatus
    {
        [Display(Name = "Do zrobienia")]
        ToDo = 1,

        [Display(Name = "W toku")]
        InProgress,

        [Display(Name = "Zakończone")]
        Complete
    }
}
