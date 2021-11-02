import React from "react";
import format from "date-fns/format";
import { makeStyles, Table } from "@material-ui/core";
import classNames from "classnames";
import { HtmlTextMessage } from "@widgetcore/BotResponse/HtmlTextMessage";
import { SingleRowSingleCell } from "@widgetcore/BotResponse/TableCell";
import Fade from "react-reveal/Fade";

const useStyles = makeStyles(theme => ({
    textField: {},
    timeStamp: {
        fontSize: "9px",
        marginTop: "0.6rem",
        marginBottom: "0.6rem",
        borderTop: "1px dashed grey",
        float: "left",
        background: "none",
    },
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
}));

export interface BotResponseProps {
    message?: string; // as HTML
    input?: React.ReactNode;
    button?: React.ReactNode;
    buttons?: React.ReactNode;
}

export const BotResponse = ({ message, input, button, buttons }: BotResponseProps) => {
    const cls = useStyles();
    return (
        <div className={cls.container}>
            <div className={cls.marker} />
            <Table className={cls.table} classes={{ root: cls.table }}>
                <>
                    {message && (
                        <SingleRowSingleCell>
                            <Fade left>
                                <HtmlTextMessage message={message} className={cls.textField} />
                            </Fade>
                        </SingleRowSingleCell>
                    )}
                    <Fade right>
                        <span className={classNames("rcw-timestamp", cls.timeStamp)}>{format(new Date(), "hh:mm")}</span>
                    </Fade>
                </>
                {input && (
                    <SingleRowSingleCell>
                        <Fade bottom>{input}</Fade>
                    </SingleRowSingleCell>
                )}
                {button && (
                    <SingleRowSingleCell align="right">
                        <Fade bottom>
                            <div className={cls.marginTop}>{button}</div>
                        </Fade>
                    </SingleRowSingleCell>
                )}
                {buttons && (
                    <Fade bottom>
                        <div className={classNames(cls.marginTop)} style={{ marginRight: "0.3rem", width: "100%" }}>
                            <div style={{ flexWrap: "wrap", display: "flex", flexDirection: "row", width: "100%", justifyContent: "left" }}>{buttons}</div>
                        </div>
                    </Fade>
                )}
            </Table>
        </div>
    );
};
