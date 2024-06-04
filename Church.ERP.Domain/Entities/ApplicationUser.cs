using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Church.ERP.Domain.Entities;
public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public  virtual  ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();

    [NotMapped]
    public override string PhoneNumber { get; set; } 
    [NotMapped]
    public override bool PhoneNumberConfirmed { get; set; } 
}

