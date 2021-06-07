namespace Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.SubscriptionCreated
{
    public static class SubscriptionCreated
    {
        public static string GetSubscriptionCreatedText()
        {
            return "Thanks so much for subscribing to Palavyr!";
        }

        public static string GetSubscriptionCreatedHtml()
        {
            return @"
<!DOCTYPE html>
<html lang='en'>
    <head>
        <meta charset='UTF-8' />
        <meta name='viewport' content='width=device-width, initial-scale=1.0' />
        <title>Palavyr Email Confirmation</title>
    </head>

    <body style='background: white; display: flex; justify-content: center; width: 100%'>
        <div id='card' style='background: #f2f2f2; width: 75%'>
            <section
                style='
                    color: white;
                    text-align: center;
                    border-radius: 7px;
                    background-color: #507fe0;
                    padding-top: 2rem;
                    padding-left: 2rem;
                    padding-right: 2rem;
                    margin-top: 2rem;
                    margin-left: 2rem;
                    margin-right: 2rem;
                '
                id='header'
            >
                <div style='text-align: center; height: 60%'>
                    <img
                        style='border-radius: 16px'
                        src='https://palavyr-public-assets.s3.amazonaws.com/LogoMedium.png'
                    />
                </div>
                <br />
            </section>
            <section
                style='
                    border-radius: 5%;
                    text-align: left;
                    margin-left: 25%;
                    padding-left: 2rem;
                    margin-right: 25%;
                    padding-right: 2rem;
                    background-color: white;
                    padding-top: 2rem;
                    margin-top: 2rem;
                '
                id='body'
            >
                <h2>Welcome to the Palavyr family!</h2>
                <p>...and congratulations on your shiny new subscription!</p>
                <div style='text-align: center'>
                    <img src='https://palavyr-public-assets.s3.amazonaws.com/thankyou.gif' />
                </div>
                <p>
                    By subscribing, you not only unlock additional features, but you provide us with the support we need
                    to keep on developing more features for you.
                </p>
                <p>
                    If you have any questions, need any help, or run into any issues while using Palavyr.com, reach out
                    to us at info.palavyr@gmail.com
                </p>
                <p>
                    Thanks again!
                </p>
                <p>
                    Sincerely,
                    <br /><br />
                    <strong> The Palavyr Team </strong>
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