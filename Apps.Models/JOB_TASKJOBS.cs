//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Apps.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class JOB_TASKJOBS
    {
        public JOB_TASKJOBS()
        {
            this.JOB_TASKJOBS_LOG = new HashSet<JOB_TASKJOBS_LOG>();
        }
    
        public string sno { get; set; }
        public string taskName { get; set; }
        public string Id { get; set; }
        public string taskTitle { get; set; }
        public string taskCmd { get; set; }
        public Nullable<System.DateTime> crtDt { get; set; }
        public Nullable<int> state { get; set; }
        public string creator { get; set; }
        public string procName { get; set; }
        public string procParams { get; set; }
    
        public virtual ICollection<JOB_TASKJOBS_LOG> JOB_TASKJOBS_LOG { get; set; }
    }
}
