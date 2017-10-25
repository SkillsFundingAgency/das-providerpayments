using ProviderPayments.TestStack.Domain;

namespace ProviderPayments.TestStack.UI.Models
{
    public class ComponentAdminModel
    {
        public Component[] Components { get; set; }
        public string UploadErrorMessage { get; set; }

        public bool HasUploadError
        {
            get { return !string.IsNullOrEmpty(UploadErrorMessage); }
        }
    }
}