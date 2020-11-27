import React from "react";
import { useStripe, useElements, CardElement } from "@stripe/react-stripe-js";
import { CardSection } from "./CardSection";
import { StripeCardElement, Token } from "@stripe/stripe-js";
import "./cardsection.css";

interface ICheckoutForm {
    stripeTokenHandler(token: Token): void;
}

export const CheckoutForm = ({ stripeTokenHandler }: ICheckoutForm) => {
    const stripe = useStripe();
    const elements = useElements();

    const handleSubmit = async (event) => {
        // We don't want to let default form submission happen here,
        // which would refresh the page.
        event.preventDefault();

        if (!stripe || !elements) {
            // Stripe.js has not yet loaded.
            // Make  sure to disable form submission until Stripe.js has loaded.
            return;
        }

        const card = elements.getElement(CardElement) as StripeCardElement;
        const result = await stripe.createToken(card);

        if (result.error) {
            // Show error to your customer.
            console.log(result.error.message);
        } else if (result.token === undefined) {
            console.log("RESULT UNDEFINED");
        } else {
            // Send the token to your server.
            // This function does not exist yet; we will define it in the next step.
            stripeTokenHandler(result.token);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <CardSection />
            <button disabled={!stripe}>Confirm order</button>
        </form>
    );
};
