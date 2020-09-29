using Server.Domain.AccountDB;

namespace Palavyr.API.Controllers
{
    
    public static class EmailConfirmationHTML
    {
        public static string GetConfirmationEmailBodyText(AccountDetails details, string confirmationId)
        {
            return $@"
Palavyr Account Setup: Email Confirmation</h2>

Hello and welcome to Palavyr!


In order to complete the sign-up process and configure your first chat, we just need to confirm
that email address!

Since you're looking at this email now, chances are, you're the same person that signed up!

To complete the process, all you need to do is use the code provided below after you sign in. This will send a
message to palavyr.com and let us know that you've confirmed that you own the email address. With that, its smooth sailing!

Account: {details.EmailAddress}

Use this code in the dashboard to confirm your email address and unlock Palavyr:

{confirmationId}

Thanks once again for joining Palavyr.com. I hope you'll enjoy our service and benefit greatly from it!


Sincerely,

Paul Gradie,
Founder
            ";
        }
        
        
        
        public static string GetConfirmationEmailBody(AccountDetails details, string confirmationId)
        {
            return $@"
                 <!DOCTYPE html>
<html lang='en'>

<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Document</title>
</head>

<body style='background: #282630; display: flex; justify-content: center;'>
    <div id='card' style='border-radius: 7px; width: 87%; background: rgb(238, 238, 238); padding: 2rem; margin: 3rem;'>
        <section id='header'>
            <h2>Palavyr Account Setup: Email Confirmation</h2>
            <br />
        </section>
        <section id='body'>
            <p>
            <h4>
                Hello and welcome to Palavyr!
            </h4>
            </p>
            <p>
                <span>
                    In order to complete the sign-up process and configure your first chat, we just need to confirm
                    that email address!

                    Since you're looking at this email now, chances are, you're the same person that signed up!
                </span>
            </p>
            <p>
                <span>
                    To complete the process, all you need to do is use the code provided below after you sign in. This will send a
                    message to palavyr.com and let us know that you've confirmed that you own the email address. With that, its smooth sailing!
                </span>
                <p>
                    Account: {details.EmailAddress}<br />
                </p>
                <span>
                    <p>
                        Use this code in the dashboard to confirm your email address and unlock Palavyr:
                    </p>
                    <br />
                    {confirmationId}
                </span>
            </p>
        </section>
        <section id='footer'>
            <p>
                Thanks once again for joining Palavyr.com. I hope you'll enjoy our service and benefit greatly from it!
            </p>
            <p>
                Sincerely,
                <br /><br />
                Paul Gradie,<br />
                Founder
            </p>
        </section>
    </div>
</body>

</html>
                    ";
        }
     
    }
}