using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Official_Vacation
    {
        private int vacationId;
        private string vacationName;
        private DateTime startDate;
        private DateTime endDate;

        public int VacationId { get => vacationId; set => vacationId = value; }
        public string VacationName { get => vacationName; set => vacationName = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
    }
}