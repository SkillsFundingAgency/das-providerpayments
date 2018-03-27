namespace SFA.DAS.Payments.AcceptanceTests.TableParsers
{
    public class FullIlrStructure
    {
        public IlrTableStructure IlrTableStructure { get; set; }
        public LearningSupportTableParser.LearningSupportTableColumnStructure LearningSupportTableColumnStructure { get; set; }
        public ContractTypeTableParser.ContractTypesTableColumnStructure ContractTypesTableColumnStructure { get; set; }
        public EmploymentStatusTableParser.EmploymentStatusTableColumnStructure EmploymentStatusTableColumnStructure { get; set; }
    }
}