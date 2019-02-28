﻿using DbModel.LocationHistory.Work;
using Location.IModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using DbModel.Tools;

namespace DbModel.Location.Work
{
    /// <summary>
    /// 巡检点
    /// </summary>
    public class PatrolPoint: IId
    {
        /// <summary>
        /// 巡检点Id
        /// </summary>
        [DataMember]
        [Display(Name = "巡检点Id")]
        public int Id { get; set; }

        /// <summary>
        /// 所属巡检轨迹ID
        /// </summary>
        [DataMember]
        [Display(Name = "所属巡检轨迹ID")]
        public int ParentId { get; set; }

        /// <summary>
        /// 巡检员工号
        /// </summary>
        [DataMember]
        [Display(Name = "巡检员工号")]
        [MaxLength(16)]
        [Required]
        public string StaffCode { get; set; }

        /// <summary>
        /// 巡检员名称
        /// </summary>
        [DataMember]
        [Display(Name = "巡检员名称")]
        [MaxLength(32)]
        public string StaffName { get; set; }

        /// <summary>
        /// 设备KKS编码
        /// </summary>
        [DataMember]
        [Display(Name = "设备KKS编码")]
        [MaxLength(16)]
        public string KksCode { get; set; }

        /// <summary>
        /// 设备在本地生成的Id
        /// </summary>
        [DataMember]
        [Display(Name = "设备在本地生成的Id")]
        public int? DevId { get; set; }


        /// <summary>
        /// 巡检设备名称
        /// </summary>
        [DataMember]
        [Display(Name = "巡检设备名称")]
        [MaxLength(128)]
        public string DevName { get; set; }

        /// <summary>
        /// 移动巡检系统中定义的设备编码
        /// </summary>
        [DataMember]
        [Display(Name = "移动巡检系统中定义的设备编码")]
        [MaxLength(32)]
        public string DeviceCode { get; set; }

        /// <summary>
        /// 移动巡检系统中定义的设备ID
        /// </summary>
        [DataMember]
        [Display(Name = "移动巡检系统中定义的设备ID")]
        [MaxLength(32)]
        public string DeviceId { get; set; }

        /// <summary>
        /// 检查项列表
        /// </summary>
        [DataMember]
        [ForeignKey("ParentId")]
        //[NotMapped]
        public virtual List<PatrolPointItem> Checks { get; set; }

        public PatrolPoint()
        {
            StaffCode = "";
            StaffName = "";
            KksCode = "";
            DevId = null;
            DevName = "";
            DeviceCode = "";
            DeviceId = "";
            Checks = new List<PatrolPointItem>();
        }

        public PatrolPointHistory ToHistory()
        {
            PatrolPointHistory history = new PatrolPointHistory();
            history.StaffCode = StaffCode;
            history.StaffName = StaffName;
            history.KksCode = KksCode;
            history.DevId = DevId;
            history.DevName = DevName;
            history.DeviceCode = DeviceCode;
            history.DeviceId = DeviceId;
            foreach (PatrolPointItem item in Checks)
            {
                PatrolPointItemHistory item2 = item.ToHistory();
                history.Checks.Add(item2);
            }
            
            return history;
        }
    }
}
