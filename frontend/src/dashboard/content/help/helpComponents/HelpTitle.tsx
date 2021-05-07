import { Divider, Typography } from "@material-ui/core";
import React from "react";

export interface IHelpTitle {
    title: string;
}
export const HelpTitle = ({ title }: IHelpTitle) => {
    return (
        <>
            <Typography align="center" variant="h4">
                {" "}
                {title}
            </Typography>
            <Divider />
        </>
    );
};
