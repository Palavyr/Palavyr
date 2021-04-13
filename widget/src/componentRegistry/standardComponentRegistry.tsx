import { assembleCompletedConvo, getChildNodes } from "./utils";
import React, { useState } from "react";
import { Table, TableRow, TableCell, makeStyles, TextField, CircularProgress, Button } from "@material-ui/core";
import { responseAction } from "./responseAction";
import CurrencyTextField from "@unicef/material-ui-currency-textfield";
import { ConvoContextProperties } from "./registry";
import { uuid } from "uuidv4";
import { IProgressTheChat, ConvoTableRow } from "@Palavyr-Types";
import { setNumIndividualsContext, getContextProperties, openUserDetails } from "@store-dispatcher";
import { ResponseButton } from "common/ResponseButton";
import { sortChildrenByOptions } from "common/sorting";
import { SingleRowSingleCell } from "common/TableCell";

const useStyles = makeStyles(() => ({
    standardContainer: {
        marginTop: "1rem",
        width: "100%",
        display: "flex",
        flexDirection: "column",
        justifyContent: "right",
    },

    table: {
        borderBottom: "none",
    },
    root: {
        borderBottom: "0px solid white",
    },
}));

export class StandardComponents {
    public makeProvideInfo({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getChildNodes(node.nodeChildrenString, nodeList)[0];
        return () => {
            const cls = useStyles();
            const [disabled, setDisabled] = useState<boolean>(false);
            return (
                <>
                    {node.text}
                    <div className={cls.standardContainer}>
                        <ResponseButton
                            text="Proceed"
                            disabled={disabled}
                            onClick={() => {
                                responseAction(node, child, nodeList, client, convoId, null);
                                setDisabled(true);
                            }}
                        />
                    </div>
                </>
            );
        };
    }

    makeMultipleChoiceContinueButtons({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getChildNodes(node.nodeChildrenString, nodeList)[0]; // only one should exist
        const valueOptions = node.valueOptions.split("|peg|");
        return () => {
            const cls = useStyles();
            const [disabled, setDisabled] = useState<boolean>(false);

            return (
                <Table>
                    {node.text}
                    <TableRow>
                        {valueOptions.map((valueOption: string) => {
                            return (
                                <TableCell className={cls.table}>
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
                            );
                        })}
                    </TableRow>
                </Table>
            );
        };
    }

    public makeMultipleChoiceAsPathButtons({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const children = getChildNodes(node.nodeChildrenString, nodeList);
        const sortedChildren = sortChildrenByOptions(children);

        return () => {
            const cls = useStyles();
            const [disabled, setDisabled] = useState<boolean>(false);

            return (
                <>
                    {node.text}
                    <Table>
                        <TableRow>
                            {sortedChildren.map((child: ConvoTableRow) => {
                                return (
                                    <TableCell className={cls.table}>
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
                                );
                            })}
                        </TableRow>
                    </Table>
                </>
            );
        };
    }

    public makeTakeNumber({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const cls = useStyles();
            const [response, setResponse] = useState<string>("");
            const [disabled, setDisabled] = useState<boolean>(false);

            return (
                <Table>
                    <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                    <TableRow>
                        <TableCell className={cls.root}>
                            <TextField
                                disabled={disabled}
                                label=""
                                type="number"
                                onChange={event => {
                                    const intValue = parseInt(event.target.value);
                                    if (!intValue) return;
                                    if (intValue < 0) return;
                                    setResponse(intValue.toString());
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
            const [disabled, setDisabled] = useState<boolean>(false);

            return (
                <>
                    <Table>
                        <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                        <SingleRowSingleCell>
                            <CurrencyTextField
                                label="Amount"
                                disabled={disabled}
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
            const cls = useStyles();
            const [response, setResponse] = useState<string>("");
            const [disabled, setDisabled] = useState<boolean>(false);

            return (
                <>
                    {node.text}
                    <Table>
                        <SingleRowSingleCell>
                            <TextField
                                disabled={disabled}
                                label="Write here..."
                                type="text"
                                onChange={event => {
                                    setResponse(event.target.value);
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
            const [disabled, setDisabled] = useState<boolean>(false);

            const cls = useStyles();

            return (
                <Table>
                    <SingleRowSingleCell>{node.text}</SingleRowSingleCell>
                    <TableRow>
                        <TableCell className={cls.root}>
                            <TextField
                                disabled={disabled}
                                label=""
                                value={response}
                                type="number"
                                onChange={event => {
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
                    {node.text}
                    <Table>
                        <TableRow>
                            <TableCell>
                                <ResponseButton
                                    text="Grant permission to send email"
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
                            </TableCell>
                        </TableRow>
                    </Table>
                    <div style={{ width: "100%", display: "flex", justifyContent: "center", textAlign: "center" }}>{loading && <CircularProgress />}</div>
                </>
            );
        };
    }

    makeRestart({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        return () => {
            const cls = useStyles();
            const noBorder = { borderBottom: "none" };
            return (
                <Table>
                    <TableRow>
                        <TableCell>{node.text}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell style={noBorder} align="right">
                            <ResponseButton
                                text="restart"
                                onClick={() => {
                                    window.location.reload();
                                }}
                            />
                        </TableCell>
                    </TableRow>
                </Table>
            );
        };
    }

    makeSendEmailFailedFirstAttempt = ({ node, nodeList, client, convoId }: IProgressTheChat) => {
        const child = getChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const cls = useStyles();
            return (
                <Table>
                    <TableRow>
                        <TableCell>{node.text}</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>
                            <ResponseButton
                                text="Check your email details."
                                variant="contained"
                                onClick={async () => {
                                    responseAction(node, child, nodeList, client, convoId, null);
                                }}
                            />
                            <Button onClick={() => openUserDetails()}>Check Details</Button>
                        </TableCell>
                    </TableRow>
                </Table>
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
                        <TableRow>
                            <TableCell>{node.text}</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>
                                <ResponseButton
                                    text="Grant permission to send email"
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
                            </TableCell>
                        </TableRow>
                    </Table>
                    <div style={{ width: "100%", display: "flex", justifyContent: "right" }}>{loading && <CircularProgress />}</div>
                </>
            );
        };
    }
}
