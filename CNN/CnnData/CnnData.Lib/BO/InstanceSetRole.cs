using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  public enum InstanceSetRoleEnum
  {
    Unassigned,
    Training,
    Validation,
    Testing
  }

  [Table("InstanceSetRoles")]
  public class InstanceSetRole
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public InstanceSetRoleEnum ID { get; set; }

    public string RoleText { get; set; }

    public static List<InstanceSetRole> GetRoles()
    {
      return new List<InstanceSetRole>() {
        new InstanceSetRole() { ID = InstanceSetRoleEnum.Unassigned, RoleText = InstanceSetRoleEnum.Unassigned.ToString() },
        new InstanceSetRole() { ID = InstanceSetRoleEnum.Training,   RoleText = InstanceSetRoleEnum.Training.ToString() },
        new InstanceSetRole() { ID = InstanceSetRoleEnum.Validation, RoleText = InstanceSetRoleEnum.Validation.ToString() },
        new InstanceSetRole() { ID = InstanceSetRoleEnum.Testing,    RoleText = InstanceSetRoleEnum.Testing.ToString() }
      };
    }
  }
}
