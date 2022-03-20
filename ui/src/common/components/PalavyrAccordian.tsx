import { makeStyles, AccordionSummary, Typography, AccordionDetails, Accordion, AccordionActions, Divider } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import { Variant } from "@material-ui/core/styles/createTypography";

const useStyles = makeStyles((theme) => ({
    accordian: {
        width: "100%",
    },
    accordianHead: {
        backgroundColor: theme.palette.primary.dark,
        color: theme.palette.getContrastText(theme.palette.primary.main),
    },
    accordianDetailsContainer: {
        width: "100%",
    },
    accordianDetails: {
        backgroundColor: "white",
        width: "100%",
        display: "flex",
    },
    accordianActions: {
        display: "flex",
        padding: "1.4rem",
        alignItems: "center",
        justifyContent: "flex-end",
        marginRight: "1.2rem"
    },
}));

export interface PalavyrAccordianProps {
    title: string;
    children?: React.ReactNode;
    initialState?: boolean;
    titleVariant?: Variant;
    actions?: React.ReactNode;
    disable?: boolean;
}

export const PalavyrAccordian = ({ title, initialState = false, titleVariant = "h5", actions, disable, children }: PalavyrAccordianProps) => {
    const [accordState, setAccordState] = useState<boolean>(false);
    const toggleAccord = () => setAccordState(!accordState);

    const cls = useStyles();

    useEffect(() => {
        setAccordState(initialState);
    }, [initialState]);

    return (
        <Accordion className={cls.accordian} expanded={accordState} disabled={disable}>
            <AccordionSummary className={cls.accordianHead} onClick={toggleAccord} expandIcon={<ExpandMoreIcon style={{ color: "white" }} />}>
                <Typography variant={titleVariant}>{title}</Typography>
            </AccordionSummary>
            <AccordionDetails className={cls.accordianDetails}>
                <div className={cls.accordianDetailsContainer}>{children}</div>
            </AccordionDetails>
            <Divider />
            {actions && <AccordionActions className={cls.accordianActions}>{actions}</AccordionActions>}
        </Accordion>
    );
};
