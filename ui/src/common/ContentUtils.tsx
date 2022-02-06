import React from "react";
import { Box, Typography } from "@material-ui/core";

export type PanelRange = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7;

export const intentTabProps = (index: PanelRange) => {
    return {
        id: `simple-tab-${index}`,
        "aria-controls": `simple-tabpanel-${index}`,
    };
};
