import { Typography } from "@material-ui/core";
import React from "react";

export interface EnquiryTimeStampProps {
    formattedDate: string;
    formattedTime: string;
}

export const EnquiryTimeStamp = ({ formattedDate, formattedTime }: EnquiryTimeStampProps) => {
    return (
        <>
            <Typography display="block" variant="caption">
                {formattedDate}
            </Typography>
            <Typography display="block" variant="caption">
                {formattedTime}
            </Typography>
        </>
    );
};
