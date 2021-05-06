import React from "react";
import { Button, PropTypes } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import { uuid } from "uuidv4";
import { Table, TableRow, TableCell, makeStyles, TextField } from "@material-ui/core";
import { format } from "date-fns";

const useStyles = makeStyles((theme) => ({
    row: {
        width: "100%",
    },
    cell: {
        border: "0px solid white",
        width: "100%",
    },
    button: (prefs: WidgetPreferences) => ({
        color: prefs.buttonFontColor,
        backgroundColor: prefs.buttonColor,
        marginBottom: "0.4rem",
        transion: "all ease-in-out 0.2s",
        border: "none",
        "&:hover": {
            color: prefs.buttonFontColor,
            backgroundColor: prefs.buttonColor,
            transition: "all ease-in-out 0.2s",
            boxShadow: theme.shadows[10],
            border: "none",
        },
    }),
    buttonFocus: (prefs: WidgetPreferences) => ({
        // color: prefs.chatFontColor,
        // borderColor: prefs.chatFontColor,
    }),
    tableCell: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        borderBottom: "none",
    },
    root: {
        borderBottom: "none",
    },
    userMessageContainer: {
        display: "flex",
        flexDirection: "column",
        marginLeft: "auto",
        maxWidth: "215px",
        textAlign: "left",
    },
    messageText: {
        padding: "15px",
        borderRadius: "10px",
        backgroundColor: "#f4f7f9",
        color: "black",
    },
    timestamp: {
        alignSelf: "flex-end",
        fontSize: "10px",
        marginTop: "5px",
    },
    textField: (prefs: WidgetPreferences) => ({
        color: prefs.chatFontColor,
    }),
}));

export interface TextProp {
    text: string;
}

export interface MultiOption extends TextProp {
    valueOptions: string[];
}

export interface IResponseButton {
    prefs: WidgetPreferences;
    onClick: any;
    disabled?: boolean;
    text?: string;
    color?: PropTypes.Color;
    variant?: "outlined" | "contained";
}

export interface IHaveNoBorder {
    align?: "left" | "right" | "center";
    children: React.ReactNode;
}

export const ResponseButton = ({ prefs, disabled = false, variant = "outlined", text = "Submit" }: IResponseButton) => {
    const cls = useStyles(prefs);
    return (
        <Button disableElevation focusVisibleClassName={cls.buttonFocus} className={cls.button} disabled={disabled} variant={variant} size="small">
            {text}
        </Button>
    );
};

export const NoBorderTableCell = ({ align, children }: IHaveNoBorder) => {
    const cls = useStyles();
    return (
        <TableCell align={align} className={cls.cell}>
            {children}
        </TableCell>
    );
};

export const SingleRowSingleCell = ({ align, children }: IHaveNoBorder) => {
    const cls = useStyles();
    return (
        <TableRow className={cls.row}>
            <NoBorderTableCell align={align}>{children}</NoBorderTableCell>
        </TableRow>
    );
};

export class StandardComponents {
    private prefs: WidgetPreferences;

    constructor(prefs: WidgetPreferences) {
        this.prefs = prefs;
    }

    public makeProvideInfo(text: string): React.ElementType<{}> {
        return () => {
            return (
                <Table>
                    <SingleRowSingleCell>{text}</SingleRowSingleCell>
                </Table>
            );
        };
    }

    makeMultipleChoiceContinueButtons(text: string, valueOptions: string[]): React.ElementType<{}> {
        const uniqId = uuid();
        return () => {
            const cls = useStyles();
            return (
                <Table>
                    <SingleRowSingleCell>{text}</SingleRowSingleCell>
                    {valueOptions.map((valueOption: string) => {
                        return (
                            <TableRow>
                                <TableCell className={cls.tableCell}>
                                    <ResponseButton prefs={this.prefs} disabled={false} key={valueOption + "-" + uniqId} text={valueOption} onClick={() => null} />
                                </TableCell>
                            </TableRow>
                        );
                    })}
                </Table>
            );
        };
    }

    public makeTakeNumber(text: string): React.ElementType<{}> {
        return () => {
            const cls = useStyles(this.prefs);
            return (
                <Table>
                    <SingleRowSingleCell>{text}</SingleRowSingleCell>
                    <TableRow>
                        <TableCell className={cls.root}>
                            <TextField InputProps={{ className: cls.textField }} disabled={false} fullWidth label="" type="number" />
                        </TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell className={cls.root} align="right">
                            <ResponseButton prefs={this.prefs} disabled={false} onClick={() => null} />
                        </TableCell>
                    </TableRow>
                </Table>
            );
        };
    }

    public makeUserMessage(text: string, timestamp: Date): React.ElementType<{}> {
        const cls = useStyles();
        return () => {
            return (
                <div className={cls.userMessageContainer}>
                    <div className={cls.messageText} dangerouslySetInnerHTML={{ __html: text }} />
                    <span className={cls.timestamp}>{format(timestamp, "hh:mm")}</span>
                </div>
            );
        };
    }
}
