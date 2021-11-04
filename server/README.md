# Palavyr Server

Palavyr Server supports the Chat Bot Designer website as well as the Chat Bot Widget. This server can be used solely to support the widget and the website in dev. The server is intended to be run as a separate instances when supporting the widget and the website in production.

For the server to run, it needs access to S3, SES, the STRIPE CLI (if you wish to execute against stripe) as well as the PalavyrPDFServer.

To set the credentials for accessing AWS, ask Paul to generate some aws credentials for you. Then set these as .net secrets using the ./utilities/setDevelopmentalSecrets.ps1 powershell script.

## Running the Stripe CLI tool

HOW TO USE STRIPE CLI WITH THIS SERVER

TO EXECUTE WEBHOOK
1. stripe login
2. stripe listen --forward-to https://localhost:5001/api/payments/payments-webhook --skip-verify
3. stripe trigger customer.subscription.created

This will forward requests to the server via the cli tool.

The way this works:
1. Request is made either via stripe.com (from my dashboard when making a subscription or from the stripe dashboard) or via the CLI
2. The request is forwarded to the stripe CLI WOOT
3. The ClI then forwards this to the server
4. The server is running https, so we need to indicate the above url (https://localhost:5001)

```
stripe listen --forward-to https://localhost:5001/api/payments/payments-webhook --skip-verify
stripe trigger invoice.payment_succeeded
```
