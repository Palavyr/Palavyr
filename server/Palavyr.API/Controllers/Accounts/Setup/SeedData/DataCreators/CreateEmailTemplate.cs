namespace Palavyr.API.Controllers.Accounts.Setup.SeedData.DataCreators
{
    public static class CreateEmailTemplate
    {
        public static string Create()
        {
            return @"
<!DOCTYPE html>
<html lang='en'>

<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Document</title>
</head>

<body style='display: flex; justify-content: center;'>
    <div id='card'
        style='border-radius: 7px; width: 700px;  padding: 2rem; margin: 3rem;'>

        <section style='margin: 0 auto; text-align: center;'>
            {%Logo%}
        </section>
        <section id='header'>
            <h2>{%Company%}</h2>
        </section>
        <section id='body'>
            <h4>
                Hi {%Name%},
                <br /><br />
                Thank you for contacting {%Company%}!
            </h4>
            <p>
                <span>
                    Your business means a lot to us, and we're glad you were able to find what you needed using our chat
                    service.
                    If there is anything else we can do to assist you, please don't hesitate to get in touch!
                </span>
            </p>
            <p>
                <span>
                    Attached with this email you will find a pdf that contains service details and our costs. If you would like to proceed,
                    you can email us at <strong>tobysEmporium@gmail.com</strong> and provide your reference estimate or the name/phone number you provided in the chat.
                </span>
            </p>
        </section>
        <section id='footer'>
            <p>
                Thanks once again for inquiring! We look forward to hearing from you!
            </p>
            <p>
                Sincerely,
                <br /><br />
                Toby Ruffnick,
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