using System;
using System.Threading.Tasks;

namespace DataLoader.Services
{
    public class SuiteTalkService
    {
        /// <summary>
        /// NOTE:
        /// These are keys are from NetSuite Training Account, and none of them contain our company's specific keys.
        /// </summary>
        const string netsuiteVersion = "2021_2";
        const string netsuiteAccountNumber = "TSTDRV2562460";
        const string netsuiteConsumerKey = "fc05e1a8481a52c6ddd7b47bc37ddaa2baee3ef75cef0ba6efd39960ce3c3f46";
        const string netsuiteConsumerSecret = "18763d9213a70a8936eefefc695ebbe56efdb3e65494bdad3f51547c70c6e792";

        const string netsuiteTokenId = "7b6191b29bada999ab07b2a8c2d6bc59a848b2d94087d4705fb9251419cc1e76";
        const string netsuiteTokenSecret = "e8fed053f1134d694bfb8583d69a271e170847041198f494cce770071e1d5ebf";
        const string applicationId = "E2D03678-5C19-4754-9774-C54B062E2CC4";
        public SuiteTalk.TokenPassport GetTokenPassport()
        {
            var hmacService = new HmacService();
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var nonce = hmacService.ComputeNonce();
            var data = string.Join("&", netsuiteAccountNumber, netsuiteConsumerKey, netsuiteTokenId, nonce, timestamp.ToString());
            var key = string.Join("&", netsuiteConsumerSecret, netsuiteTokenSecret);


            var encodedSignature = hmacService.Sign256(data, key);


            Console.WriteLine("Voila! A signature: " + encodedSignature);

            var signature = new SuiteTalk.TokenPassportSignature { algorithm = "HMAC_SHA256", Value = encodedSignature };

            return new SuiteTalk.TokenPassport
            {
                account = netsuiteAccountNumber,
                consumerKey = netsuiteConsumerKey,
                nonce = nonce,
                token = netsuiteTokenId,
                signature = signature,
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
        }


        public SuiteTalk.ApplicationInfo GetApplicationInfo()
        {
            return new SuiteTalk.ApplicationInfo
            {
                applicationId = ""
            };
        }


        public SuiteTalk.PartnerInfo GetPartnerInfo()
        {
            return new SuiteTalk.PartnerInfo
            {
            };
        }

        public SuiteTalk.Preferences GetPreferences()
        {
            return new SuiteTalk.Preferences
            {

            };
        }
        //public NetSuiteService _service;
        //private Preferences _prefs;
        //private SearchPrefrences _searchPreferences;
        public async Task GetEmployee()
        {
            var a = new SuiteTalk.NetSuitePortTypeClient(SuiteTalk.NetSuitePortTypeClient.EndpointConfiguration.NetSuitePort,
                "https://tstdrv2562460.suitetalk.api.netsuite.com/services/NetSuitePort_2021_2");
            var result = await a.getAsync(
                GetTokenPassport(),
                GetApplicationInfo(),
                GetPartnerInfo(),
                GetPreferences(),
                new SuiteTalk.RecordRef
                {
                    internalId = "-5",
                    type = SuiteTalk.RecordType.employee,
                    typeSpecified = true
                });

            if (result.readResponse.status.isSuccess)
            {

            }
            else
            {
                foreach (var d in result.readResponse.status.statusDetail)
                {

                }
            }
        }

    }
}
// Please note: The keys below are from Training Account, not from our company's web.

//Warning: For security reasons, this is the only time that the Client Credentials values are displayed. After you leave this page, they cannot be retrieved from the system. If you lose or forget these credentials, you will need to reset them to obtain new values.
//Treat the values for Client Credentials as you would a password.Never share these credentials with unauthorized individuals and never send them by email.
//CONSUMER KEY / CLIENT ID
//fc05e1a8481a52c6ddd7b47bc37ddaa2baee3ef75cef0ba6efd39960ce3c3f46
//CONSUMER SECRET / CLIENT SECRET
//18763d9213a70a8936eefefc695ebbe56efdb3e65494bdad3f51547c70c6e792


//Warning: For security reasons, this is the only time that the Token ID and Token Secret values are displayed. After you leave this page, they cannot be retrieved from the system. If you lose or forget these credentials, you will need to reset them to obtain new values.
//Treat the values for Token ID and Token Secret as you would a password.Never share these credentials with unauthorized individuals and never send them by email.
//TOKEN ID
//7b6191b29bada999ab07b2a8c2d6bc59a848b2d94087d4705fb9251419cc1e76
//TOKEN SECRET
//e8fed053f1134d694bfb8583d69a271e170847041198f494cce770071e1d5ebf