import { makeStyles, Theme, Typography } from "@material-ui/core";
import React from "react";

export interface PalavyrText {
    text: string;
}

const useStyles = makeStyles((theme: Theme) => {
    type: {
        fontSize: "12px"
    }
})

export const PalavyrText = ({ text }) => {
    return <Typography>{text}</Typography>;
};
