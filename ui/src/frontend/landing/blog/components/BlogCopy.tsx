import { Typography } from "@material-ui/core";
import React from "react";

export const BlogCopy = (props) => {
    return (
        <Typography variant="body2" gutterBottom style={{ paddingBottom: "1rem" }}>
            {props.children}
        </Typography>
    );
};
