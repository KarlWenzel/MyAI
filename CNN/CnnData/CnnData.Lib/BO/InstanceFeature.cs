﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnnData.Lib.BO
{
  [Table("InstanceFeatures")]
  public class InstanceFeature
  {
    [Key, Column(Order = 0)]
    [MaxLength(128)]
    [ForeignKey("FeatureType")]
    public string FeatureName { get; set; }
    public virtual FeatureType FeatureType { get; set; }

    [Key, Column(Order = 1)]
    [ForeignKey("Instance")]
    public int InstanceID { get; set; }
    public virtual Instance Instance { get; set; }

    public string Value { get; set; }
  }
}
