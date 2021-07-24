import { Typography } from "@material-ui/core";
import React from "react";

export interface NoDataMessageProps {
    text: string;
}
export const NoDataAvailable = ({ text }: NoDataMessageProps) => {
    return (
        <div style={{ paddingTop: "3rem" }}>
            <Typography align="center" variant="h4">
                {text}
            </Typography>
        </div>
    );
};
