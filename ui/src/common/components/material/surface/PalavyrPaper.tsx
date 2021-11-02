import { Paper, PaperProps } from "@material-ui/core";
import React from "react";

interface PalavyrPaperProps extends PaperProps {}

export const PalavyrPaper = ({ children, ...rest }: PalavyrPaperProps) => {
    return <Paper {...rest}>{children}</Paper>;
};
