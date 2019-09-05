using System;
using System.Collections.Generic;

namespace NorthwindCore.Entities
{
    public partial class Employment
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Title { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? LeaveDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
