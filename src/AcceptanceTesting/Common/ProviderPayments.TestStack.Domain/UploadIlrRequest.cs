namespace ProviderPayments.TestStack.Domain
{
    public class UploadIlrRequest
    {
        public string Id { get; set; }
        public byte[] Data { get; set; }
        public string YearOfCollection { get; set; }
    }
}
