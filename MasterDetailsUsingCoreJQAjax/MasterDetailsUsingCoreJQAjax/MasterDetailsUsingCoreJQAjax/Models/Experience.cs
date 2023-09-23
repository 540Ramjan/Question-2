using System.Reflection.Metadata.Ecma335;

namespace MasterDetailsUsingCoreJQAjax.Models
{
    public class Experience
    {
        public int ExperienceId { get; set; }
        public string CompanyName { get; set;}
        public int YearsWorked { get; set;}
        public int ApplicantId { get; set;}
        public virtual Applicant Applicant { get; set; }
    }
}
