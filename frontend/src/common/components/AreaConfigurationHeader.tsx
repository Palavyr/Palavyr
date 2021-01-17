import { Typography } from "@material-ui/core";
import React from "react";

interface IAreaConfigurationHeader {
    title: string;
    subtitle: string;
}

export const AreaConfigurationHeader = ({ title, subtitle }: IAreaConfigurationHeader) => {
    return (
        <>
            <Typography style={{ marginTop: "1.4rem" }} align="center" variant="h4">
                {title}
            </Typography>
            <Typography paragraph align="center">
                {subtitle}
            </Typography>
        </>
    );
};
