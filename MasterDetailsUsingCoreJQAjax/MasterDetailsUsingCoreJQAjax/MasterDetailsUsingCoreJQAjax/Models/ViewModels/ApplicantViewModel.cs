using System.ComponentModel.DataAnnotations;

namespace MasterDetailsUsingCoreJQAjax.Models.ViewModels
{
    public class ApplicantViewModel
    {
        [Key]
        public int ApplicantId { get; set; }
        [Required]
        public string ApplicantName { get; set; }
        [Required, Display(Name = "Date Of Birth"), DataType(DataType.Date)]
        public DateTime DOB { get; set; } = DateTime.Now;
        public string MobileNo { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int DesignationId { get; set; }      
        public int ExperienceId { get; set; }        
        public IList<Designation> Designations { get; set; }
        public virtual IList<Experience> Experiences { get; set; }=new List<Experience>();        
        public IFormFile ProfilePhoto { get; set; }

    }
}
