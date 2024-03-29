﻿using System;
using Humanizer;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers.PaymentFailed
{
    public class EmailPaymentFailed
    {
        public static string GetPaymentFailedEmailText(DateTime endDate)
        {
            return $"Your subscription will lapse {endDate.Humanize()} on {endDate.ToString("D")}";
        }

        public static string GetPaymentFailedEmailHtml(DateTime endDate)
        {
            return $@"
<!DOCTYPE html>
<html lang='en'>

<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Palavyr Email Confirmation</title>
</head>

<body style=' background: white; display: flex; justify-content: center; width: 100%'>
    <div id='card' style='background: #F2F2F2; width: 75%;'>
        <section
            style='color: white; text-align: center; border-radius: 7px; background-color: #507FE0; padding-top: 2rem; padding-left: 2rem; padding-right: 2rem; margin-top: 2rem; margin-left: 2rem; margin-right: 2rem;'
            id='header'>
            <div style='text-align: center; height: 60%;'><img style='border-radius: 16px;'
                    src='https://palavyr-public-assets.s3.amazonaws.com/LogoMedium.png' /></div>
            <br />
        </section>
        <section
            style='border-radius: 5%;  text-align: left; margin-left: 25%; padding-left: 2rem; margin-right: 25%; background-color: white; padding-top: 2rem; margin-top: 2rem;'
            id='body'>
            <h2>
                Oh No! Your latest subscription payment has failed!
            </h2>
            <p>
                Don't worry, we aren't going to cancel your subscription yet or anything :D
            </p>
            <br />
            <p>
                We typically allow a buffer period to allow you to update your payment method and try again. We'll continue trying to charge your current payment method just in case there was a temporary problem.
            </p>
            <p>
                Your subscription will lapse {endDate.Humanize()} on {endDate.ToString("D")}
            </p>
            <p>
            <p>
                In the meantime, if you wish to continue using Palavyr, you can log into your account and navigate to your payment details using the 'Manage' tab in the sidebar navigation menu.
            </p>
            <p>
                If you have any questions or need any help, reach out to us at info.palavyr@gmail.com
            </p>
            <p>
                Sincerely,
                <br /><br />
                <strong>
                    The Palavyr Team
                </strong>
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