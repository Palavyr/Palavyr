import React from "react";
import { PricingCardTableRow } from "./PricingCardTableRow";

export interface PricingCardBody {
    response: string | React.ReactNode;
    perAreaEmail: string | React.ReactNode;
    enquiriesDashboard: string | React.ReactNode;
    fileAssetUpload: string | React.ReactNode;
    emailNotifications: string | React.ReactNode;
    inlineEmailEditor: string | React.ReactNode;
    smsNotifications: string | React.ReactNode;
    attachmentsPerArea: string | React.ReactNode;
    staticFeeTables: string | React.ReactNode;
    dynamicFeeTables: string | React.ReactNode;
    numberOfAreas: string | React.ReactNode;
    rowStyle?: {};
    textStyle?: {};
}

export const PricingCardBody = ({
    response,
    perAreaEmail,
    enquiriesDashboard,
    fileAssetUpload,
    emailNotifications,
    inlineEmailEditor,
    smsNotifications,
    attachmentsPerArea,
    staticFeeTables,
    dynamicFeeTables,
    numberOfAreas,
    rowStyle,
    textStyle,
}: PricingCardBody) => {
    return (
        <>
            <PricingCardTableRow textStyle={textStyle} rowStyle={rowStyle} itemName="PDF Response" value={response} />
            <PricingCardTableRow textStyle={textStyle} rowStyle={rowStyle} itemName="Per Intent Email" value={perAreaEmail} />
            <PricingCardTableRow textStyle={textStyle} rowStyle={rowStyle} itemName="Enquiries Dashboard" value={enquiriesDashboard} />
            <PricingCardTableRow textStyle={textStyle} rowStyle={rowStyle} itemName="File Upload" value={fileAssetUpload
    } />
            <PricingCardTableRow textStyle={textStyle} rowStyle={rowStyle} itemName="Email Notifications" value={emailNotifications} />
            <PricingCardTableRow textStyle={textStyle} rowStyle={rowStyle} itemName="Inline Email Editor" value={inlineEmailEditor} />
            <PricingCardTableRow textStyle={textStyle} rowStyle={rowStyle} itemName="SMS Notifications" value={smsNotifications} />
            <PricingCardTableRow textStyle={textStyle} rowStyle={rowStyle} itemName="Attachments per Intent" value={attachmentsPerArea} />
            <PricingCardTableRow textStyle={textStyle} rowStyle={rowStyle} itemName="Static Fee Tables" value={staticFeeTables} />
            <PricingCardTableRow textStyle={textStyle} rowStyle={rowStyle} itemName="Pricing Strategies" value={dynamicFeeTables} />
            <PricingCardTableRow textStyle={textStyle} rowStyle={rowStyle} itemName="Number of Intents" value={numberOfAreas} />
        </>
    );
};
