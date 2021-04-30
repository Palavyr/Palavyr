import React from "react";
import { PricingCardTableRow } from "./PricingCardTableRow";

export interface PricingCardBody {
    response: string | React.ReactNode;
    email: string | React.ReactNode;
    area: string | React.ReactNode;
    enquiries: string | React.ReactNode;
    attachments: string | React.ReactNode;
    table: string | React.ReactNode;
    editor: string | React.ReactNode;
}

export const PricingCardBody = ({ response, email, area, enquiries, attachments, table, editor }: PricingCardBody) => {
    return (
        <>
            <PricingCardTableRow left="Areas" right={area} />
            <PricingCardTableRow left="Static Fee Tables" right={table} />
            <PricingCardTableRow left="PDF Response" right={response} />
            <PricingCardTableRow left="Configurable Email" right={email} />
            <PricingCardTableRow left="Inline Email Editor" right={editor} />
            <PricingCardTableRow left="Enquiries Dashboard" right={enquiries} />
            <PricingCardTableRow left="Attachments per Area" right={attachments} />
        </>
    );
};
