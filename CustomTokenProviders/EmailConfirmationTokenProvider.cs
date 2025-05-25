using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Trustesse_Assessment.CustomTokenProviders
{
    public class EmailConfirmationTokenProvider<TAppUser> : DataProtectorTokenProvider<TAppUser> where TAppUser : class
    {
        public EmailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider,
            IOptions<EmailConfirmationTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<TAppUser>> logger)
            : base(dataProtectionProvider, options, logger)
        {
        }
    }
    public class EmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
    {
    }
}
