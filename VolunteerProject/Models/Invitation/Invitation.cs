using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace VolunteerProject.Models
{
    public class Invitation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdInv {  get; set; }

        [Required]
        public int OrganizationId { get; set; }

        [Required]
        public int VolunteerId { get; set; }
        
        public Volunteer Volunteer { get; set; }
        public Organization Organization { get; set; }
    }
}
