using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Automation.Application.Entities
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class OrderAttribute : Attribute
    {
        private readonly int order_;
        public OrderAttribute(int order = 0)
        {
            order_ = order;
        }

        public int Order { get { return order_; } }
    }
    public class CommitmentBulkLoadEntity
    {
        [Order(0)]
        public string CohortRef { get; set; } = "COHORTREF";
        [Order(1)]
        public string Uln { get; set; }
        [Order(2)]
        public string FamilyName { get; set; }
        [Order(3)]
        public string GivenNames { get; set; }
        [Order(4)]
        public string DateOfBirth { get; set; }
        [Order(5)]
        public string ProgType { get; set; }
        [Order(6)]
        public string FworkCode { get; set; }
        [Order(7)]
        public string PwayCode { get; set; }
        [Order(8)]
        public string StdCode { get; set; }
        [Order(9)]
        public string StartDate { get; set; }
        [Order(10)]
        public string EndDate { get; set; }
        [Order(11)]
        public string TotalPrice { get; set; }
        [Order(12)]
        public string EPAOrgID { get; set; } = "";
        [Order(13)]
        public string ProviderRef { get; set; } = Guid.NewGuid().ToString();
        
    }
}
