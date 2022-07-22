using Humanizer;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers.InvoiceCreated
{
    public static class InvoiceCreatedEmail
    {
        public static string GetInvoiceCreatedEmailText(string currency, string amountDue, string dueDate)
        {
            return $"Your next invoice. Amount: {currency}{amountDue} - Due Date: {dueDate.Humanize()} on {dueDate}";
        }

        public static string GetInvoiceCreatedEmailHtml(string currency, string amountDue, string dueDate)
        {
            return $@"
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
                <div style='text-align: center'>
                    <h2>Your next invoice</h2>
                </div>
                <div style='text-align: center'>
                    <span>(Whenever you are ready)</span>
                    <img src='https://palavyr-public-assets.s3.amazonaws.com/whenYouAreReady.gif' width='400px'/>
                </div>
                <p></p>
                <p>
                    Amount Due: {currency}{amountDue}.00
                </p>
                <p>
                    Due Date: {dueDate}
                </p>
                <p>
                    This invoice will be automatically billed at the end of this billing period.
                </p>
                <p>
                    If you have any questions, need any help, or run into any issues while using Palavyr.com, reach out
                    to us at info.palavyr@gmail.com
                </p>
                <p>Thanks again for subscribing!</p>
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