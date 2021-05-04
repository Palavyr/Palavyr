import React, { ElementType } from "react";
import { Box, Card, makeStyles, TextField, Typography } from "@material-ui/core";
import { Autocomplete, AutocompleteRenderInputParams } from "@material-ui/lab";
import classNames from "classnames";
import { SpaceEvenly } from "dashboard/layouts/positioning/SpaceEvenly";
import SettingsIcon from "@material-ui/icons/Settings";
import { StandardComponents } from "./standardComponentDuplicate";
import format from "date-fns/format";
import { WidgetPreferences } from "@Palavyr-Types";

const useStyles = makeStyles((theme) => ({
    selectListBgColor: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.selectListColor,
    }),
    selectListFontColor: (prefs: WidgetPreferences) => ({
        color: prefs.listFontColor,
    }),
    selectbox: {
        paddingLeft: "2rem",
        paddingRight: "2rem",
    },
    autocomplete: {
        paddingTop: "1rem",
    },
    mainList: {
        height: "40%",
    },
    root: {
        "& .MuiAutocomplete-popper": {
            backgroundColor: "black",
            zIndex: 99999999,
        },
    },
    inputLabel: (prefs: WidgetPreferences) => ({
        "& .MuiFormLabel-root": {
            color: prefs.listFontColor,
            fontSize: "10pt",
        },
    }),
    frame: (props) => ({
        height: "500px",
        width: "380px",
        display: "table",
        borderRadius: "9px",
        border: "0px",
        background: "#FFFFFF",
        boxShadow: theme.shadows[10],
    }),
    header: (prefs: WidgetPreferences) => ({
        display: "tableCell",
        backgroundColor: prefs.headerColor,
        color: prefs.headerFontColor,
        textAlign: "center",
        minWidth: 275,
        wordWrap: "break-word",
        borderRadius: "5px",
        borderBottomLeftRadius: "0px",
        borderBottomRightRadius: "0px",
    }),
    headerBehavior: {
        wordWrap: "break-word",
        padding: "1rem",
        width: "100%",
        wordBreak: "normal",
        minHeight: "18%",
    },
    widgetBody: {
        display: "tableCell",
        width: "100%",
        height: "74%",
        borderBottomRightRadius: "5px",
        borderBottomLeftRadius: "5px",
    },
    settingsIcon: {
        position: "fixed",
        right: "5px",
        top: "5px",
        height: "2rem",
        width: "2rem",
    },
    messageText: (props: StyleProps) => makeChatBodyColor(props),
    layout: {
        textAlign: "left",
        overflowX: "scroll",
    },
    messagesContainer: {
        backgroundColor: theme.palette.common.white,
        maxHeight: "410px",
        overflowY: "scroll",
        paddingTop: "10px",
        "-webkit-overflow-scrolling": "touch",
    },

    message: {
        margin: "10px",
        display: "flex",
        wordWrap: "break-word",
    },
}));

type StyleProps = {
    backgroundColor: string;
    color: string;
};

const makeChatBodyColor = (props: StyleProps) => {
    let chatBodyStyles = {
        backgroundColor: "#F4F4F4",
        color: "white",
        paddingLeft: "0.2rem",
        paddingRight: "0.2rem",
        paddingTop: "0.1rem",
        borderRadius: "10px",
        maxWidth: "85%",
    };

    if (props.backgroundColor) {
        chatBodyStyles = { ...chatBodyStyles, backgroundColor: props.backgroundColor };
    }
    if (props.color) {
        chatBodyStyles = { ...chatBodyStyles, color: props.color };
    }
    return chatBodyStyles;
};

export interface IWrapMessages {
    customPreferences: WidgetPreferences;
    children: React.ReactNode;
}

export const MessageWrapper = ({ customPreferences, children }: IWrapMessages) => {
    const cls = useStyles({ color: customPreferences.chatFontColor, backgroundColor: customPreferences.chatBubbleColor });
    return <Box className={classNames(cls.messageText, cls.layout)}>{children}</Box>;
};

export const getComponentToRender = (message: FakeMessage, customPreferences: WidgetPreferences) => {
    const ComponentToRender = message.component;
    if (message.sender === "response") {
        return (
            <MessageWrapper customPreferences={customPreferences}>
                <ComponentToRender />
            </MessageWrapper>
        );
    }
    return <ComponentToRender />;
};

export const FakeWidget = ({ ...prefs }: WidgetPreferences) => {
    const cls = useStyles(prefs);
    const reg = new StandardComponents(prefs);

    return (
        <div style={{ margin: "3rem" }}>
            <SpaceEvenly>
                <div style={{ marginRight: "3rem" }}>
                    <Typography align="center" variant="h6">
                        (Landing Screen)
                    </Typography>
                    <div className={cls.frame} id="landingWidgetPanel">
                        <div style={{ display: "tableRow" }}>
                            <Card className={cls.header}>
                                <div className={cls.headerBehavior} dangerouslySetInnerHTML={{ __html: prefs.landingHeader }} />
                            </Card>
                        </div>
                        <div className={classNames(cls.selectListBgColor, cls.selectListFontColor, cls.widgetBody)}>
                            <Autocomplete
                                size="small"
                                classes={{ root: cls.selectbox, paper: classNames(cls.selectListBgColor, cls.selectListFontColor) }}
                                disableClearable
                                clearOnEscape
                                open={true}
                                className={classNames(cls.root, cls.autocomplete, cls.mainList, cls.selectListBgColor, cls.selectListFontColor)}
                                onChange={(x) => null}
                                options={[{ x: "First Option" }, { x: "Second Option" }, { x: "Third Option" }]}
                                getOptionLabel={(option) => option.x}
                                renderInput={(params: AutocompleteRenderInputParams) => (
                                    <TextField
                                        {...params}
                                        className={cls.inputLabel}
                                        label="Select an area or start typing..."
                                        inputProps={{
                                            ...params.inputProps,
                                            autoComplete: "new-password",
                                        }}
                                    />
                                )}
                            />
                        </div>
                    </div>
                </div>
                <div style={{ marginLeft: "3rem" }}>
                    <Typography align="center" variant="h6">
                        (Chat Screen)
                    </Typography>
                    <div className={cls.frame} id="chatWidgetPanel">
                        <Card className={cls.header}>
                            <SettingsIcon className={cls.settingsIcon} onClick={() => null} />
                            <div className={cls.headerBehavior} dangerouslySetInnerHTML={{ __html: prefs.chatHeader }} />
                        </Card>
                        <div className={classNames(cls.messagesContainer, cls.widgetBody)}>
                            {fakeMessages(reg).map((message: FakeMessage, index: number) => (
                                <div className={cls.message} key={`${index}-${format(message.timestamp, "hh:mm")}`}>
                                    {getComponentToRender(message, prefs)}
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </SpaceEvenly>
        </div>
    );
};

export type FakeMessage = {
    component: ElementType;
    sender: string;
    timestamp: Date;
    unread: boolean;
};

const firstMessage = (reg: StandardComponents): FakeMessage => ({
    component: reg.makeProvideInfo("Hello"),
    sender: "response",
    timestamp: new Date(),
    unread: false,
});

const firstClient = (reg: StandardComponents): FakeMessage => ({
    component: reg.makeUserMessage("Hi there", new Date()),
    sender: "client",
    timestamp: new Date(),
    unread: false,
});

const secondMessage = (reg: StandardComponents): FakeMessage => ({
    component: reg.makeProvideInfo("Thanks for your interest."),
    sender: "response",
    timestamp: new Date(),
    unread: false,
});

const secondClient = (reg: StandardComponents): FakeMessage => ({
    component: reg.makeUserMessage("Well you are welcome!", new Date()),
    sender: "client",
    timestamp: new Date(),
    unread: false,
});

const thirdMessage = (reg: StandardComponents): FakeMessage => ({
    component: reg.makeMultipleChoiceContinueButtons("Choose an option. There are a few options to choose from", ["First option", "Second option", "Another third Option"]),
    sender: "response",
    timestamp: new Date(),
    unread: false,
});

const fakeMessages = (reg: StandardComponents) => [firstMessage(reg), firstClient(reg), secondMessage(reg), secondClient(reg), thirdMessage(reg)];
