using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.ERP.Domain.Entities;

public class PhoneNumber
{
    public Guid Id { get; set; }
    public string Extension { get; set; } = "";
    public string Number { get; set; } = "";
    public PhoneNumberType Type { get; set; }

    public Guid ApplicationUserId { get; set; }
    public virtual ApplicationUser  ApplicationUser { get; set; }

}

public enum PhoneNumberType
{
    Home = 0,
    Work = 1,
    Mobile = 2,
    Others = 3,
}
