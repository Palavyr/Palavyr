import { assembleCompletedConvo, getOrderedChildNodes } from "./utils";
import React, { useEffect, useState } from "react";
import { Table, TableRow, TableCell, makeStyles, TextField, Typography } from "@material-ui/core";
import { responseAction } from "./responseAction";
import CurrencyTextField from "@unicef/material-ui-currency-textfield";
import { ConvoContextProperties } from "./registry";
import { ContextProperties, DynamicResponses, IProgressTheChat, WidgetNodeResource, WidgetPreferences } from "@Palavyr-Types";
import { setNumIndividualsContext, getContextProperties, openUserDetails, getWidgetPreferences } from "@store-dispatcher";
import { ResponseButton } from "common/ResponseButton";
import { SingleRowSingleCell } from "common/TableCell";
import { splitValueOptionsByDelimiter } from "widget/utils/valueOptionSplitter";
import { ChatLoadingSpinner } from "common/UserDetailsDialog/ChatLoadingSpinner";
import { uuid } from "uuidv4";

const useStyles = makeStyles(theme => ({
    tableCell: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        borderBottom: "none",
    },
    root: {
        borderBottom: "none",
    },
    textField: (prefs: WidgetPreferences) => ({
        color: prefs.chatFontColor,
        borderColor:  theme.palette.getContrastText(prefs.chatBubbleColor ?? "black"),
    }),
    textLabel: (prefs: WidgetPreferences) => ({
        color: theme.palette.getContrastText(prefs.chatBubbleColor ?? "black"),
        "&:focus": {
            color: theme.palette.getContrastText(prefs.chatBubbleColor ?? "black"),
        },
    }),
}));

export class StandardComponents {
    public makeProvideInfo({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];
        const prefs = getWidgetPreferences();

        return () => {
            const cls = useStyles(prefs);
            useEffect(() => {
                setTimeout(() => {
                    responseAction(node, child, nodeList, client, convoId, null);
                }, 1500);
            }, []);

            return (
                <Table>
                    <SingleRowSingleCell>
                        <Typography variant="body1" className={cls.textField}>
                            {node.text}
                        </Typography>
                    </SingleRowSingleCell>
                </Table>
            );
        };
    }

    makeMultipleChoiceContinueButtons({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0]; // only one should exist
        const valueOptions = splitValueOptionsByDelimiter(node.valueOptions);
        const prefs = getWidgetPreferences();

        return () => {
            const cls = useStyles(prefs);
            const [disabled, setDisabled] = useState<boolean>(false);

            return (
                <Table>
                    <SingleRowSingleCell>
                        <Typography variant="body1" className={cls.textField}>
                            {node.text}
                        </Typography>
                    </SingleRowSingleCell>
                    {valueOptions.map((valueOption: string) => {
                        return (
                            <TableRow>
                                <TableCell className={cls.tableCell}>
                                    <ResponseButton
                                        prefs={prefs!}
                                        disabled={disabled}
                                        key={valueOption + "-" + uuid()}
                                        text={valueOption}
                                        onClick={() => {
                                            const response = valueOption;
                                            responseAction(node, child, nodeList, client, convoId, response);
                                            setDisabled(true);
                                        }}
                                    />
                                </TableCell>
                            </TableRow>
                        );
                    })}
                </Table>
            );
        };
    }

    public makeMultipleChoiceAsPathButtons({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const children = getOrderedChildNodes(node.nodeChildrenString, nodeList);
        // const sortedChildren = sortChildrenByOptions(children);
        const prefs = getWidgetPreferences();

        return () => {
            const cls = useStyles(prefs);
            const [disabled, setDisabled] = useState<boolean>(false);

            return (
                <Table>
                    <SingleRowSingleCell>
                        <Typography variant="body1" className={cls.textField}>
                            {node.text}
                        </Typography>
                    </SingleRowSingleCell>
                    {children.map((child: WidgetNodeResource) => {
                        return (
                            <TableRow>
                                <TableCell className={cls.tableCell}>
                                    {child.optionPath && (
                                        <ResponseButton
                                            prefs={prefs!}
                                            disabled={disabled}
                                            key={child.nodeId}
                                            text={child.optionPath}
                                            onClick={() => {
                                                var response = child.optionPath;
                                                responseAction(node, child, nodeList, client, convoId, response);
                                                setDisabled(true);
                                            }}
                                        />
                                    )}
                                </TableCell>
                            </TableRow>
                        );
                    })}
                </Table>
            );
        };
    }

    public makeTakeNumber({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        // With numbers, we have the potential for exceeding some minimum or maximum value.
        let child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];
        const prefs = getWidgetPreferences();

        return () => {
            const cls = useStyles(prefs);
            const [response, setResponse] = useState<string>("");
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);

            return (
                <Table>
                    <SingleRowSingleCell>
                        <Typography variant="body1" className={cls.textField}>
                            {node.text}
                        </Typography>
                    </SingleRowSingleCell>
                    <TableRow>
                        <TableCell className={cls.root}>
                            <TextField
                                InputProps={{
                                    className: cls.textField,
                                }}
                                InputLabelProps={{
                                    className: cls.textLabel,
                                }}
                                disabled={inputDisabled}
                                fullWidth
                                label=""
                                type="number"
                                onChange={event => {
                                    const intValue = parseInt(event.target.value);
                                    if (!intValue) return;
                                    if (intValue < 0) return;
                                    setResponse(intValue.toString());
                                    setDisabled(false);
                                }}
                            />
                        </TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell className={cls.root} align="right">
                            <ResponseButton
                                prefs={prefs!}
                                disabled={disabled}
                                onClick={async () => {
                                    // will need to do something like this. -- this might all go into the response action..

                                    if (node.isDynamicTableNode && node.dynamicType && node.resolveOrder && node.resolveOrder > 0) {
                                        // we have some kind of dynamic table node that may or may not
                                        const contextProperties: ContextProperties = getContextProperties();
                                        const dynamicResponses = contextProperties[ConvoContextProperties.dynamicResponses] as DynamicResponses;

                                        // const tableId = extractDynamicTypeGuid(node.dynamicType);
                                        const currentDynamicResponseState = dynamicResponses.filter(x => Object.keys(x)[0] === node.dynamicType)[0];

                                        // send the dynamic responses, the
                                        const { data: tooComplicated } = await client.Widget.Post.InternalCheck(node, response, currentDynamicResponseState);
                                        if (tooComplicated) {
                                            child = nodeList.filter(x => x.nodeType === "TooComplicated")[0];
                                        }
                                    }

                                    responseAction(node, child, nodeList, client, convoId, response);
                                    setDisabled(true);
                                    setInputDisabled(true);
                                }}
                            />
                        </TableCell>
                    </TableRow>
                </Table>
            );
        };
    }

    makeTakeCurrency({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];
        const prefs = getWidgetPreferences();

        return () => {
            const cls = useStyles(prefs);
            const [response, setResponse] = useState<number>(0);
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);
            return (
                <>
                    <Table>
                        <SingleRowSingleCell>
                            <Typography variant="body1" className={cls.textField}>
                                {node.text}
                            </Typography>
                        </SingleRowSingleCell>
                        <SingleRowSingleCell>
                            <CurrencyTextField
                                InputProps={{
                                    className: cls.textField,
                                }}
                                InputLabelProps={{
                                    className: cls.textLabel,
                                }}
                                className={cls.tableCell}
                                label="Amount"
                                disabled={inputDisabled}
                                variant="standard"
                                value={response}
                                currencySymbol="$"
                                minimumValue="0"
                                outputFormat="number"
                                decimalCharacter="."
                                digitGroupSeparator=","
                                onChange={(event: any, value: number) => {
                                    if (value !== undefined) {
                                        setResponse(value);
                                        setDisabled(false);
                                    }
                                }}
                            />
                        </SingleRowSingleCell>
                        <SingleRowSingleCell align="right">
                            <ResponseButton
                                prefs={prefs!}
                                disabled={disabled}
                                onClick={() => {
                                    responseAction(node, child, nodeList, client, convoId, response.toString());
                                    setDisabled(true);
                                    setInputDisabled(true);
                                }}
                            />
                        </SingleRowSingleCell>
                    </Table>
                </>
            );
        };
    }

    makeTakeText({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];
        const prefs = getWidgetPreferences();

        return () => {
            const [response, setResponse] = useState<string>("");
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);
            const cls = useStyles(prefs);
            return (
                <>
                    <Table>
                        <SingleRowSingleCell>
                            <Typography variant="body1" className={cls.textField}>
                                {node.text}
                            </Typography>
                        </SingleRowSingleCell>{" "}
                        <SingleRowSingleCell>
                            <TextField
                                InputProps={{
                                    className: cls.textField,
                                }}
                                InputLabelProps={{
                                    className: cls.textLabel,
                                }}
                                fullWidth
                                multiline
                                disabled={inputDisabled}
                                label="Write here..."
                                type="text"
                                onChange={event => {
                                    setResponse(event.target.value);
                                    setDisabled(false);
                                }}
                            />
                        </SingleRowSingleCell>
                        <SingleRowSingleCell align="right">
                            <ResponseButton
                                prefs={prefs!}
                                disabled={disabled || response === ""}
                                text="Submit"
                                onClick={() => {
                                    setResponse(response);
                                    responseAction(node, child, nodeList, client, convoId, response);
                                    setDisabled(true);
                                    setInputDisabled(true);
                                }}
                            />
                        </SingleRowSingleCell>
                    </Table>
                </>
            );
        };
    }

    makeTakeNumberIndividuals({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];
        const prefs = getWidgetPreferences();

        return () => {
            const [response, setResponse] = useState<number | null>(null);
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);

            const cls = useStyles(prefs);

            return (
                <Table>
                    <SingleRowSingleCell>
                        <Typography variant="body1" className={cls.textField}>
                            {node.text}
                        </Typography>
                    </SingleRowSingleCell>{" "}
                    <TableRow>
                        <TableCell className={cls.root}>
                            <TextField
                                InputProps={{
                                    className: cls.textField,
                                }}
                                InputLabelProps={{
                                    className: cls.textLabel,
                                }}
                                disabled={inputDisabled}
                                label=""
                                value={response}
                                type="number"
                                onChange={event => {
                                    setDisabled(false);
                                    const intValue = parseInt(event.target.value);
                                    if (!intValue) return;
                                    if (intValue < 0) return;
                                    setResponse(intValue);
                                }}
                            />
                        </TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell className={cls.root} align="right">
                            <ResponseButton
                                prefs={prefs!}
                                disabled={disabled}
                                onClick={() => {
                                    if (response) {
                                        responseAction(node, child, nodeList, client, convoId, response.toString());
                                        setNumIndividualsContext(response);
                                        setDisabled(true);
                                        setInputDisabled(true);
                                    }
                                }}
                            />
                        </TableCell>
                    </TableRow>
                </Table>
            );
        };
    }

    makeSendEmail({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const areaId = nodeList[0].areaIdentifier;
        const prefs = getWidgetPreferences();

        const sendEmail = async () => {
            const contextProperties = getContextProperties();
            const email = contextProperties[ConvoContextProperties.emailAddress];
            const name = contextProperties[ConvoContextProperties.name];
            const phone = contextProperties[ConvoContextProperties.phoneNumber];

            let numIndividuals = contextProperties[ConvoContextProperties.numIndividuals];
            let dynamicResponses = contextProperties[ConvoContextProperties.dynamicResponses];
            let keyvalues = contextProperties[ConvoContextProperties.keyValues];

            if (!keyvalues) {
                keyvalues = [];
            }
            if (!dynamicResponses) {
                dynamicResponses = [];
            }

            if (!numIndividuals) {
                numIndividuals = 1;
            }

            const { data: response } = await client.Widget.Send.ConfirmationEmail(areaId, email, name, phone, numIndividuals, dynamicResponses, keyvalues, convoId);
            if (response.result) {
                const completeConvo = assembleCompletedConvo(convoId, areaId, name, email, phone);
                await client.Widget.Post.CompletedConversation(completeConvo);
            }
            return response;
        };

        return () => {
            const cls = useStyles(prefs);
            const [disabled, setDisabled] = useState<boolean>(false);
            const [loading, setLoading] = useState<boolean>(false);
            return (
                <>
                    <Table>
                        <SingleRowSingleCell>
                            <Typography variant="body1" className={cls.textField}>
                                {node.text}
                            </Typography>
                        </SingleRowSingleCell>{" "}
                        <SingleRowSingleCell align="center">
                            <ResponseButton
                                prefs={prefs!}
                                text="Send my email"
                                variant="contained"
                                disabled={disabled}
                                onClick={async () => {
                                    setLoading(true);
                                    const response = await sendEmail();
                                    const child = nodeList.filter((x: WidgetNodeResource) => x.nodeId === response.nextNodeId)[0];
                                    responseAction(node, child, nodeList, client, convoId, null, () => setLoading(false));
                                    setDisabled(true);
                                }}
                            />
                        </SingleRowSingleCell>
                    </Table>
                    <ChatLoadingSpinner loading={loading} />
                </>
            );
        };
    }

    makeRestart({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const prefs = getWidgetPreferences();

        return () => {
            const cls = useStyles(prefs);
            return (
                <Table>
                    <SingleRowSingleCell>
                        <Typography variant="body1" className={cls.textField}>
                            {node.text}
                        </Typography>
                    </SingleRowSingleCell>{" "}
                    <SingleRowSingleCell align="right">
                        <ResponseButton
                            prefs={prefs!}
                            text="restart"
                            onClick={() => {
                                // setSelectedOption(null);
                                // dropMessages();
                                // TODO: can reset the widget gby dumping messages and by putting seSelectedOption into redux and setting to null
                                window.location.reload();
                            }}
                        />
                    </SingleRowSingleCell>
                </Table>
            );
        };
    }

    makeSendEmailFailedFirstAttempt = ({ node, nodeList, client, convoId }: IProgressTheChat) => {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];
        const prefs = getWidgetPreferences();

        return () => {
            const [loading, setLoading] = useState<boolean>(false);
            const cls = useStyles(prefs);
            return (
                <>
                    <Table>
                        <SingleRowSingleCell>
                            <Typography variant="body1" className={cls.textField}>
                                {node.text}
                            </Typography>
                        </SingleRowSingleCell>{" "}
                        <SingleRowSingleCell align="center">
                            <ResponseButton
                                prefs={prefs!}
                                text="Send my email"
                                variant="contained"
                                onClick={async () => {
                                    setLoading(true);
                                    responseAction(node, child, nodeList, client, convoId, null, () => setLoading(false));
                                }}
                            />
                            <ResponseButton prefs={prefs!} text="Check your details" variant="contained" onClick={() => openUserDetails()} />
                        </SingleRowSingleCell>
                    </Table>
                    <ChatLoadingSpinner loading={loading} />
                </>
            );
        };
    };

    makeSendFallbackEmail({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const areaId = nodeList[0].areaIdentifier;
        const prefs = getWidgetPreferences();

        return () => {
            const cls = useStyles(prefs);
            const [disabled, setDisabled] = useState<boolean>(false);
            const [loading, setLoading] = useState<boolean>(false);

            const sendFallbackEmail = async () => {
                const contextProperties = getContextProperties();

                const email = contextProperties[ConvoContextProperties.emailAddress];
                const name = contextProperties[ConvoContextProperties.name];
                const phone = contextProperties[ConvoContextProperties.phoneNumber];

                const { data: response } = await client.Widget.Send.FallbackEmail(areaId, email, name, phone, convoId);
                if (response.result) {
                    var completeConvo = assembleCompletedConvo(convoId, areaId, name, email, phone);
                    await client.Widget.Post.CompletedConversation(completeConvo);
                }
                return response;
            };
            return (
                <>
                    <Table>
                        <SingleRowSingleCell>
                            <Typography variant="body1" className={cls.textField}>
                                {node.text}
                            </Typography>
                        </SingleRowSingleCell>{" "}
                        <SingleRowSingleCell align="center">
                            <ResponseButton
                                prefs={prefs!}
                                text="Send my email"
                                variant="contained"
                                disabled={disabled}
                                onClick={async () => {
                                    setLoading(true);
                                    const response = await sendFallbackEmail();
                                    const child = nodeList.filter((x: WidgetNodeResource) => x.nodeId === response.nextNodeId)[0];
                                    responseAction(node, child, nodeList, client, convoId, null, () => setLoading(false));
                                    setDisabled(true);
                                }}
                            />
                        </SingleRowSingleCell>
                    </Table>
                    <ChatLoadingSpinner loading={loading} />
                </>
            );
        };
    }
}
