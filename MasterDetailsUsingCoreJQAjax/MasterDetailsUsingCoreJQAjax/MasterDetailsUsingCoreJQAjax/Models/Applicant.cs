namespace MasterDetailsUsingCoreJQAjax.Models
{
    public class Applicant
    {
        public int ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public DateTime DOB { get; set; }
        public string MobileNo { get; set;}
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int DesignationId { get; set; }
        public virtual Designation Designation { get; set; }
        public virtual ICollection<Experience> Experiences { get; set; }
    }
}
