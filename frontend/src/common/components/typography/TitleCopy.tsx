import { Typography } from "@material-ui/core";
import React from "react";

export const TitleCopy = (props) => {
    return (
        <Typography variant="h5" paragraph>
            {props.children}
        </Typography>
    );
};
