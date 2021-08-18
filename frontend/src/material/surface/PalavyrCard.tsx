import { Card, CardProps } from "@material-ui/core";
import React from "react";

interface PalavyrCardProps extends CardProps {}

export const PalavyrCard = ({ children, ...rest }: PalavyrCardProps) => {
    return <Card {...rest}>{children}</Card>;
};
