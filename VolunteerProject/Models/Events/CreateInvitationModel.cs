using System.ComponentModel.DataAnnotations;

namespace VolunteerProject.Models
{
    public class CreateInvitationModel
    {
        [Required]
        public int VolunteerId { get; set; }
    }
}
