using Microsoft.Xrm.Sdk;
using System.Runtime.Serialization;

namespace DataverseAIFunctions
{
    [DataContract(Name = "OrganizationRequest", Namespace = "http://schemas.microsoft.com/xrm/2011/Contracts")]
    public class AIClassifyRequest : OrganizationRequest
    {
        public string[] Categories
        {
            get
            {
                return (string[])Parameters["Categories"];
            }
            set
            {
                Parameters["Categories"] = value;
            }
        }
        public string Text
        {
            get
            {
                return (string)Parameters["Text"];
            }
            set
            {
                Parameters["Text"] = value;
            }
        }
        public AIClassifyRequest() : base()
        {
            RequestName = "AIClassify";
        }
    }
}
