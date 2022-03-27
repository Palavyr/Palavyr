import React, { useContext } from "react";
import format from "date-fns/format";
import { makeStyles, Table, TableBody, TableRow } from "@material-ui/core";
import classNames from "classnames";
import { HtmlTextMessage } from "@widgetcore/BotResponse/HtmlTextMessage";
import { SingleRowSingleCell } from "@widgetcore/BotResponse/TableCell";
// import Fade from "react-reveal/Fade";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { WidgetPreferences } from "@Palavyr-Types";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

const useStyles = makeStyles(theme => ({
    textField: {},
    timeStamp: (props: WidgetPreferences) => ({
        fontSize: "9px",
        marginTop: "0.6rem",
        marginBottom: "0.6rem",
        borderTop: "1px dashed grey",
        float: "left",
        background: "none",
        fontFamily: props.fontFamily,
    }),
    container: {
        display: "flex",
        background: "none",
        width: "100%",
    },
    marker: {
        position: "relative",
        borderLeft: "30px",
        borderColor: "black",
        top: "0px",
        height: "3rem",
    },
    table: {
        margin: "0px",
        padding: "0px",
        marginLeft: "0.5rem",
        background: "none",
        width: "100%",
    },
    marginTop: {
        marginTop: "1rem",
    },
    pdfLinkContainer: {
        display: "flex",
        justifyContent: "flex-center",
        alignItems: "center",
        padding: ".5rem",
        margin: "0.5rem",
        color: "black",
        textAlign: "center",
        backgroundColor: "lightgrey",
        textDecoration: "none",
        "&:hover": {
            background: "gray",
            color: "white",
            borderRadius: "10px",
        },
    },
    link: (props: WidgetPreferences) => ({
        fontFamily: props.fontFamily,
        textDecoration: "none",
        textAlign: "center",
        "&:active": {
            textDecoration: "none",
        },
    }),
}));

export interface BotResponseProps {
    message?: string; // as HTML
    input?: React.ReactNode;
    button?: React.ReactNode;
    buttons?: React.ReactNode;
    pdfLink?: string | null;
}

export const BotResponse = ({ message, input, button, buttons, pdfLink = null }: BotResponseProps) => {
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles(preferences);
    return (
        <div className={cls.container}>
            <div className={cls.marker} />
            <Table className={cls.table} classes={{ root: cls.table }}>
                <TableBody>
                    <>
                        {message && (
                            <SingleRowSingleCell>
                                {/* <Fade left> */}
                                <HtmlTextMessage message={message} className={cls.textField} />
                                {/* </Fade> */}
                            </SingleRowSingleCell>
                        )}
                        {pdfLink && (
                            <div className={cls.pdfLinkContainer}>
                                <PalavyrText style={{ textDecoration: "none" }} align="center">
                                    <a href={pdfLink} target="_blank" className={cls.link}>
                                        Click to view your estimate now
                                    </a>
                                </PalavyrText>
                            </div>
                        )}
                        {/* <Fade right> */}
                        <TableRow className={classNames("rcw-timestamp", cls.timeStamp)}>{format(new Date(), "hh:mm")}</TableRow>
                        {/* </Fade> */}
                    </>
                    {input && (
                        <SingleRowSingleCell>
                            {/* <Fade bottom> */}
                            {input}
                            {/* </Fade> */}
                        </SingleRowSingleCell>
                    )}
                    {button && (
                        <SingleRowSingleCell align="right">
                            <div className={cls.marginTop}>{button}</div>
                        </SingleRowSingleCell>
                    )}
                    {buttons && (
                        // <Fade bottom>
                        <div className={classNames(cls.marginTop)} style={{ marginRight: "0.3rem", width: "100%" }}>
                            <div style={{ flexWrap: "wrap", display: "flex", flexDirection: "row", width: "100%", justifyContent: "evenly" }}>{buttons}</div>
                        </div>
                        // </Fade>
                    )}
                </TableBody>
            </Table>
        </div>
    );
};
