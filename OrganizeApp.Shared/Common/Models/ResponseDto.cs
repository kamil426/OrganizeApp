using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizeApp.Shared.Common.Models
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Errors { get; set; }
    }
}
