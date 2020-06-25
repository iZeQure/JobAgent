﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data
{
    public class ConsultantArea : BaseEntity
    {
        #region Attributes
        private string name;
        private string description;
        #endregion

        #region Properties
        public string Name { get { return name; } set { name = value; } }
        public string Description {  get { return description; } set { description = value; } }
        #endregion
    }
}
