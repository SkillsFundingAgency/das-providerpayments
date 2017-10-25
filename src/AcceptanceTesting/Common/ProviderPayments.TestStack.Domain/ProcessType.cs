namespace ProviderPayments.TestStack.Domain
{
    public enum ProcessType
    {
        SubmitIlr,
        UploadIlr,
        RunSummarisation,
        RebuildDedsDatabase,
        RunAccountsReferenceData,
        RunCommitmentsReferenceData
    }
}