using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class ScheduleConfig
    {
        public int ScheduleId { get; set; }
        public string ReportName { get; set; } = string.Empty;
        public TimeOnly SendTime { get; set; } = new TimeOnly(8, 0);
        public string SendTimeString
        {
            get => SendTime.ToString("HH:mm");
            set => SendTime = TimeOnly.Parse(value);
        }

        public string FrequencyType { get; set; } = "Daily";
        public int TimesPerDay { get; set; } = 1;
        public DateTime? NextRunDateTime { get; set; }
        public int Status { get; set; } = 0;
        public string? CronExpression { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
