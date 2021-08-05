import { Typography } from "@material-ui/core";
import React from "react";

export interface ITourText {
    children: React.ReactNode;
}

export const TourText = ({ children }: ITourText) => {
    return (
        <Typography align="center" variant="body2">
            {children}
        </Typography>
    );
};
