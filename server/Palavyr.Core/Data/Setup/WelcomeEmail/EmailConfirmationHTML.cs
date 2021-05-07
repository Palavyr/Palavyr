namespace Palavyr.Core.Data.Setup.WelcomeEmail
{
    
    public static class EmailConfirmationHtml
    {
        public static string GetConfirmationEmailBodyText(string emailAddress, string confirmationId)
        {
            return $@"
Palavyr Confirmation Email

Welcome to Palavyr!

Before we begin, we just need to confirm that email address!

Account: {emailAddress}<br />

To confirm your email, provide the following token to plavyr.com after you sign in.

{confirmationId}

With that, its smooth sailing!

Thanks once again for joining Palavyr.com. We hope you'll enjoy our service and benefit greatly from it!


Sincerely,
The Palavyr Team

            ";
        }

        public static string GetConfirmationEmailBody(string emailAddress, string confirmationId)
        {
            return $@"
<!DOCTYPE html>
<html lang='en'>

<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Palavyr Email Confirmation</title>
    <link
        href='https://fonts.googleapis.com/css2?family=Kanit:ital,wght@0,400;0,500;1,600&family=Poppins:ital,wght@0,200;0,300;0,400;0,500;1,100;1,200;1,300&family=Rubik:wght@700&display=swap'
        rel='stylesheet'>
</head>

<body style='font-family: Kanit; background: white; display: flex; justify-content: center;'>
    <div id='card' style='background: #F2F2F2; width: 75%;'>
        <section
            style='color: white; text-align: center; border-radius: 7px; background-color: #507FE0; padding-top: 2rem; padding-left: 2rem; padding-right: 2rem; margin-top: 2rem; margin-left: 2rem; margin-right: 2rem;'
            id='header'>
            <h2>Palavyr Confirmation Email</h2>
            <br />
        </section>
        <section
            style='font-family: Poppins; background-color: white; padding-top: 2rem; padding-left: 2rem; padding-right: 2rem; margin-top: 2rem; margin-left: 2rem; margin-right: 2rem;'
            id='body'>
            <h4>
                Welcome to Palavyr!
            </h4>
            <p>
                Before we begin, we just need to confirm that email address!
            </p>
            <p>
                Account: {emailAddress}<br />
            </p>
            <p>
                To confirm your email, provide the following token to plavyr.com after you sign in.
            </p>
            <p>
                {confirmationId}
            </p>
            <p>
                With that, its smooth sailing!
            </p>

            <p>
                Thanks once again for joining Palavyr.com. We hope you'll enjoy our service and benefit greatly from it!
            </p>
            <p>
                Sincerely,
                <br /><br />
                <strong>
                    The Palavyr Team
                </strong>,
                <br />
                <br />
                <br />
            </p>
        </section>
    </div>
</body>

</html>
                    ";
        }
     
    }
}