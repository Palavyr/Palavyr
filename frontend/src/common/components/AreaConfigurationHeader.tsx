import { Divider, Typography } from "@material-ui/core";
import React from "react";

interface IAreaConfigurationHeader {
    title: string;
    subtitle: string;
    divider?: boolean;
}

export const AreaConfigurationHeader = ({ title, subtitle, divider = false }: IAreaConfigurationHeader) => {
    return (
        <>
            <div style={{ width: "100%" }}>
                <Typography style={{ marginTop: "1.4rem" }} align="center" variant="h4">
                    {title}
                </Typography>
                <Typography paragraph align="center">
                    {subtitle}
                </Typography>
            </div>
            {divider && <Divider />}
        </>
    );
};
