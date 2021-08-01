// using System.Linq;
// using Palavyr.Core.Common.Environment;
// using Palavyr.Core.Exceptions;
//
// namespace Palavyr.Core.Data.CompanyData
// {
//     public interface IAllowedUsers
//     {
//         bool IsEmailAllowedToCreateAccount(string emailAddress);
//         bool IsATestStripeEmail(string emailAddress);
//     }
//
//     public class AllowedUsers : IAllowedUsers
//     {
//         private readonly IDetermineCurrentEnvironment currentEnvironment;
//
//         public AllowedUsers(IDetermineCurrentEnvironment currentEnvironment)
//         {
//             this.currentEnvironment = currentEnvironment;
//         }
//         
//         public static string[] StagingWhiteList =>
//             new[]
//             {
//                 "paul.e.gradie@gmail.com",
//                 "palavyr.demo@gmail.com",
//                 "ana.gradie@gmail.com",
//                 "anasadeghi15@gmail.com"
//             };
//
//         public bool IsATestStripeEmail(string emailAddress)
//         {
//             return !StagingWhiteList.Contains(emailAddress);
//         }
//         
//         // TODO: Extend this later to autogenerate a database of emails to ignore (malicious accounts, etc)
//         public bool IsEmailAllowedToCreateAccount(string emailAddress)
//         {
//             if (currentEnvironment.IsDevelopment() || currentEnvironment.IsProduction()) return true;
//             if (currentEnvironment.IsStaging())
//             {
//                 if (StagingWhiteList.Contains(emailAddress)) return true;
//                 return false;
//             }
//
//             throw new DomainException("Could not determine the current environment");
//         }
//     }
// }