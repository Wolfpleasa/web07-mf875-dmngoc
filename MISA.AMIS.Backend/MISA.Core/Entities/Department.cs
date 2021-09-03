using MISA.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class Department : BaseEntity
    {
        #region Property
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Mã đơn vị
        /// </summary>
        [MISARequired ("Mã đơn vị")]
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Tên đơn vị
        /// </summary>
        [MISARequired("Tên đơn vị")]
        public string DepartmentName { get; set; }
        #endregion
    }
}
