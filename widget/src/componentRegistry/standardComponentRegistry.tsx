import { assembleCompletedConvo, getChildNodes } from "./utils";
import React, { useEffect, useState } from "react";
import { Table, TableRow, TableCell, makeStyles, TextField } from "@material-ui/core";
import { responseAction } from "./responseAction";
import CurrencyTextField from "@unicef/material-ui-currency-textfield";
import { ConvoContextProperties } from "./registry";
import { uuid } from "uuidv4";
import { IProgressTheChat, ConvoTableRow } from "@Palavyr-Types";
import { setNumIndividualsContext, getContextProperties, openUserDetails, dropMessages, toggleWidget } from "@store-dispatcher";
import { ResponseButton } from "common/ResponseButton";
import { SingleRowSingleCell } from "common/TableCell";
import { splitValueOptionsByDelimiter } from "widget/utils/valueOptionSplitter";
import { ChatLoadingSpinner } from "common/UserDetailsDialog/ChatLoadingSpinner";

const useStyles = makeStyles(() => ({
    tableCell: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        borderBottom: "none",
    },
    root: {
        borderBottom: "none",
    },
}));

export class StandardComponents {
    public makeProvideInfo({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getChildNodes(node.nodeChildrenString, nodeList)[0];
        return () => {
            useEffect(() => {
                setTimeout(() => {
                    responseAction(node, child, nodeList, client, convoId, null);
                }, 1500);
            }, []);

            return (
                <Table>
                    <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                </Table>
            );
        };
    }

    makeMultipleChoiceContinueButtons({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getChildNodes(node.nodeChildrenString, nodeList)[0]; // only one should exist
        const valueOptions = splitValueOptionsByDelimiter(node.valueOptions);
        return () => {
            const cls = useStyles();
            const [disabled, setDisabled] = useState<boolean>(false);

            return (
                <Table>
                    <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                    {valueOptions.map((valueOption: string) => {
                        return (
                            <TableRow>
                                <TableCell className={cls.tableCell}>
                                    <ResponseButton
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
        const children = getChildNodes(node.nodeChildrenString, nodeList);
        // const sortedChildren = sortChildrenByOptions(children);

        return () => {
            const cls = useStyles();
            const [disabled, setDisabled] = useState<boolean>(false);

            return (
                <Table>
                    <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                    {children.map((child: ConvoTableRow) => {
                        return (
                            <TableRow>
                                <TableCell className={cls.tableCell}>
                                    {child.optionPath && (
                                        <ResponseButton
                                            disabled={disabled}
                                            key={child.id}
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
        const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const cls = useStyles();
            const [response, setResponse] = useState<string>("");
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);

            return (
                <Table>
                    <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                    <TableRow>
                        <TableCell className={cls.root}>
                            <TextField
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
                                disabled={disabled}
                                onClick={() => {
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
        const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const cls = useStyles();
            const [response, setResponse] = useState<number>(0);
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);
            return (
                <>
                    <Table>
                        <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                        <SingleRowSingleCell>
                            <CurrencyTextField
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
        const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const [response, setResponse] = useState<string>("");
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);

            return (
                <>
                    <Table>
                        <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                        <SingleRowSingleCell>
                            <TextField
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
        const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const [response, setResponse] = useState<number | null>(null);
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);

            const cls = useStyles();

            return (
                <Table>
                    <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                    <TableRow>
                        <TableCell className={cls.root}>
                            <TextField
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
            const cls = useStyles();
            const [disabled, setDisabled] = useState<boolean>(false);
            const [loading, setLoading] = useState<boolean>(false);
            return (
                <>
                    <Table>
                        <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                        <SingleRowSingleCell align="center">
                            <ResponseButton
                                text="Send my email"
                                variant="contained"
                                disabled={disabled}
                                onClick={async () => {
                                    setLoading(true);
                                    const response = await sendEmail();
                                    const child = nodeList.filter((x: ConvoTableRow) => x.nodeId === response.nextNodeId)[0];
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
        return () => {
            return (
                <Table>
                    <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                    <SingleRowSingleCell align="right">
                        <ResponseButton
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
        const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const [loading, setLoading] = useState<boolean>(false);

            return (
                <>
                    <Table>
                        <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                        <SingleRowSingleCell align="center">
                            <ResponseButton
                                text="Send my email"
                                variant="contained"
                                onClick={async () => {
                                    setLoading(true);
                                    responseAction(node, child, nodeList, client, convoId, null, () => setLoading(false));
                                }}
                            />
                            <ResponseButton text="Check your details" variant="contained" onClick={() => openUserDetails()} />
                        </SingleRowSingleCell>
                    </Table>
                    <ChatLoadingSpinner loading={loading} />
                </>
            );
        };
    };

    makeSendFallbackEmail({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const areaId = nodeList[0].areaIdentifier;

        return () => {
            const cls = useStyles();
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
                        <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                        <SingleRowSingleCell align="center">
                            <ResponseButton
                                text="Send my email"
                                variant="contained"
                                disabled={disabled}
                                onClick={async () => {
                                    setLoading(true);
                                    const response = await sendFallbackEmail();
                                    const child = nodeList.filter((x: ConvoTableRow) => x.nodeId === response.nextNodeId)[0];
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
