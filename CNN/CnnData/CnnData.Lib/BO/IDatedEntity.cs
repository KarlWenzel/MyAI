using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  public interface IDatedEntity
  {
    DateTime? CreatedOn { get; set; }
    DateTime? UpdatedOn { get; set; }
  }
}
