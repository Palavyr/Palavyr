import { GA4R } from "ga-4-react";
import React from "react";

export const Test: React.FC<any> = ({ ga4 }) => {
    return <>{ga4 && console.log(ga4)}</>;
};

export const TestTrack = () => {
    return (
        <GA4R code="G-9RFNBGK7HW">
            <Test></Test>
        </GA4R>
    );
};
