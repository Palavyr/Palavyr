import { assembleEmailRecordData, getOrderedChildNodes, getRootNode, MinNumeric, parseNumericResponse } from "../BotResponse/utils/utils";
import React, { useContext, useEffect, useState } from "react";
import { Button, makeStyles } from "@material-ui/core";
import { responseAction } from "../BotResponse/utils/responseAction";
import { ConvoContextProperties } from "./registry";
import { AreaTable, GlobalState, IProgressTheChat, LocaleMap, LocaleResource, SelectedOption, WidgetNodeResource, WidgetPreferences } from "@Palavyr-Types";
import { setNumIndividualsContext, getContextProperties, openUserDetails, setRegionContext, getNameContext, getEmailAddressContext } from "@store-dispatcher";
import { ResponseButton } from "widget/BotResponse/ResponseButton";
import { splitValueOptionsByDelimiter } from "widget/utils/valueOptionSplitter";
import { ChatLoadingSpinner } from "common/UserDetailsDialog/ChatLoadingSpinner";
import { CustomImage } from "widget/BotResponse/CustomImage";
import { NumberFormatValues } from "react-number-format";
import { TextInput } from "widget/BotResponse/number/TextInput";
import { BotResponse } from "../BotResponse/BotResponse";
import { WidgetContext } from "widget/context/WidgetContext";
import { useLocation } from "react-router-dom";
import { PalavyrWidgetRepository } from "client/PalavyrWidgetRepository";
import { useSelector } from "react-redux";
import { useCallback } from "react";
import { renderNextComponent } from "../BotResponse/utils/renderNextComponent";
import { ChoiceList } from "widget/BotResponse/optionFormats/ChoiceList";
import { ContactForm, MiniContactForm } from "common/UserDetailsDialog/CollectDetailsForm";
import CheckCircleOutlineIcon from "@material-ui/icons/CheckCircleOutline";
import { CurrencyTextField } from "widget/BotResponse/numbers/CurrencyTextField";

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
        textDecoration: "none",
        color: prefs.chatFontColor,
        borderColor: prefs.chatBubbleColor ? theme.palette.getContrastText(prefs.chatBubbleColor) : "black",
    }),
    textLabel: (prefs: WidgetPreferences) => ({
        color: prefs.chatBubbleColor ? theme.palette.getContrastText(prefs.chatBubbleColor) : "black",
        "&:focus": {
            color: prefs.chatBubbleColor ? theme.palette.getContrastText(prefs.chatBubbleColor) : "black",
        },
    }),
    image: {
        height: "100%",
        width: "100%",
        borderRadius: "5px",
        padding: "0.2rem",
        "&:hover": {
            cursor: "pointer",
        },
    },
}));

export class StandardComponents {
    public makeSelectOptions({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        return () => {
            const [selectedOption, setSelectedOption] = useState<SelectedOption | null>(null);
            const [options, setOptions] = useState<Array<SelectedOption>>();
            const [disabled, setDisabled] = useState<boolean>(false);
            const [open, setOpen] = useState<boolean>(false);

            const loadAreas = useCallback(async () => {
                var areas = await client.Widget.Get.Areas();
                var options = areas.map((area: AreaTable) => {
                    return { areaDisplay: area.areaDisplayTitle, areaId: area.areaIdentifier };
                });

                setOptions(options);
            }, []);

            useEffect(() => {
                loadAreas();
            }, [loadAreas]);

            const onChange = async (_: any, newOption: SelectedOption) => {
                const newConversation = await client.Widget.Get.NewConversation(newOption.areaId, { Name: getNameContext(), Email: getEmailAddressContext() });
                const nodes = newConversation.conversationNodes;
                const convoId = newConversation.conversationId;
                const rootNode = getRootNode(nodes);
                setDisabled(true);
                renderNextComponent(rootNode, nodes, client, convoId);
            };

            return (
                <BotResponse
                    message={node.text}
                    input={
                        <div style={{ marginTop: "2rem", marginBottom: "2rem", width: "100%" }}>
                            <ChoiceList disabled={disabled} onChange={onChange} setOpen={setOpen} options={options} open={open} />
                        </div>
                    }
                />
            );
        };
    }

    public makeCollectDetails({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const { setChatStarted, setConvoId } = useContext(WidgetContext);

            const [disabled, setDisabled] = useState<boolean>(false);

            const [status, setStatus] = useState<string | null>(null);
            const [detailsSet, setDetailsSet] = useState<boolean>(false);

            const onFormSubmit = (e: { preventDefault: () => void }) => {
                e.preventDefault();
                setDisabled(true);
                setChatStarted(true);
                setConvoId(convoId);
                responseAction(node, child, nodeList, client, convoId, null);
            };

            return (
                <BotResponse
                    message={node.text}
                    input={
                        <MiniContactForm
                            disabled={disabled}
                            onFormSubmit={onFormSubmit}
                            setDetailsSet={setDetailsSet}
                            formProps={{ status, setStatus }}
                            submitButton={<ResponseButton disabled={disabled} onSubmit={onFormSubmit} type="submit" />}
                        />
                    }
                />
            );
        };
    }

    public makeProvideInfo({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];
        return () => {
            useEffect(() => {
                responseAction(node, child, nodeList, client, convoId, null);
            }, []);

            return <BotResponse message={node.text} />;
        };
    }

    public makeMultipleChoiceContinueButtons({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0]; // only one should exist
        const valueOptions = splitValueOptionsByDelimiter(node.valueOptions);

        return () => {
            const [disabled, setDisabled] = useState<boolean>(false);
            return (
                <BotResponse
                    message={node.text}
                    buttons={
                        <div style={{ display: "flex", flexDirection: "row", width: "100%", justifyContent: "flex-start" }}>
                            {valueOptions.map((valueOption: string, index: number) => {
                                return (
                                    <ResponseButton
                                        disabled={disabled}
                                        key={valueOption + "-" + index}
                                        text={valueOption}
                                        onClick={() => {
                                            const response = valueOption;
                                            responseAction(node, child, nodeList, client, convoId, response);
                                            setDisabled(true);
                                        }}
                                    />
                                );
                            })}
                        </div>
                    }
                />
            );
        };
    }

    public makeMultipleChoiceAsPathButtons({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const children = getOrderedChildNodes(node.nodeChildrenString, nodeList);

        return () => {
            const [disabled, setDisabled] = useState<boolean>(false);
            return (
                <BotResponse
                    message={node.text}
                    buttons={
                        <>
                            {children.map((child: WidgetNodeResource) => {
                                return (
                                    <>
                                        {child.optionPath && (
                                            <ResponseButton
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
                                    </>
                                );
                            })}
                        </>
                    }
                />
            );
        };
    }

    public makeTakeNumber({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        let child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const [response, setResponse] = useState<string>("");
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);
            const cls = useStyles();
            return (
                <BotResponse
                    message={node.text}
                    input={
                        <TextInput
                            label=""
                            inputPropsClassName={cls.textField}
                            inputLabelPropsClassName={cls.textLabel}
                            onChange={event => {
                                setResponse(parseNumericResponse(event.target.value));
                                setDisabled(false);
                            }}
                        />
                    }
                    button={
                        <ResponseButton
                            disabled={disabled}
                            onClick={async () => {
                                responseAction(node, child, nodeList, client, convoId, response);
                                setDisabled(true);
                                setInputDisabled(true);
                            }}
                        />
                    }
                />
            );
        };
    }

    makeTakeCurrency({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const { preferences } = useContext(WidgetContext);
            const cls = useStyles(preferences);
            const [response, setResponse] = useState<number>(0);
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);
            return (
                <BotResponse
                    message={node.text}
                    input={
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
                            onValueChange={(values: NumberFormatValues) => {
                                if (values.floatValue !== undefined) {
                                    setResponse(values.floatValue);
                                    setDisabled(false);
                                }
                            }}
                        />
                    }
                    button={
                        <ResponseButton
                            disabled={disabled}
                            onClick={() => {
                                responseAction(node, child, nodeList, client, convoId, response.toString());
                                setDisabled(true);
                                setInputDisabled(true);
                            }}
                        />
                    }
                />
            );
        };
    }

    makeShowImage({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const [loaded, setLoaded] = useState<boolean>(false);
            const [link, setLink] = useState<string>("");

            useEffect(() => {
                (async () => {
                    const presignedUrl = await client.Widget.Get.NodeImage(node.nodeId);
                    setLink(presignedUrl);
                    setLoaded(false);
                })();

                setTimeout(() => {
                    responseAction(node, child, nodeList, client, convoId, null);
                }, 4500);
            }, []);

            return <CustomImage imageLink={link} />;
        };
    }

    makeTakeText({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const [response, setResponse] = useState<string>("");
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);
            const cls = useStyles();
            return (
                <BotResponse
                    message={node.text}
                    input={
                        <TextInput
                            inputPropsClassName={cls.textField}
                            inputLabelPropsClassName={cls.textLabel}
                            disabled={inputDisabled}
                            onChange={event => {
                                setResponse(event.target.value);
                                setDisabled(false);
                            }}
                        />
                    }
                    button={
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
                    }
                />
            );
        };
    }

    makeTakeNumberIndividuals({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const [response, setResponse] = useState<number | null>(null);
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);
            const cls = useStyles();
            return (
                <BotResponse
                    message={node.text}
                    input={
                        <TextInput
                            inputPropsClassName={cls.textField}
                            inputLabelPropsClassName={cls.textLabel}
                            disabled={inputDisabled}
                            label=""
                            value={response}
                            type="number"
                            onChange={event => {
                                setDisabled(false);
                                const intValue = parseInt(event.target.value);
                                if (!intValue) return;
                                if (intValue < MinNumeric) {
                                    setResponse(MinNumeric);
                                } else {
                                    setResponse(intValue);
                                }
                            }}
                        />
                    }
                    button={
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
                    }
                />
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
            const locale = contextProperties[ConvoContextProperties.region];

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

            const response = await client.Widget.Send.ConfirmationEmail(areaId, email, name, phone, numIndividuals, dynamicResponses, keyvalues, convoId);
            if (response.result) {
                const completeConvo = assembleEmailRecordData(convoId, areaId, name, email, phone, locale);
                await client.Widget.Post.UpdateConvoRecord(completeConvo);
            }
            return response;
        };

        return () => {
            const [disabled, setDisabled] = useState<boolean>(false);
            const [loading, setLoading] = useState<boolean>(false);
            return (
                <>
                    <BotResponse
                        message={node.text}
                        button={
                            <ResponseButton
                                text="Send my email"
                                variant="contained"
                                disabled={disabled}
                                onClick={async () => {
                                    setDisabled(true);
                                    setLoading(true);
                                    const response = await sendEmail();
                                    const child = nodeList.filter((x: WidgetNodeResource) => x.nodeId === response.nextNodeId)[0];
                                    responseAction(node, child, nodeList, client, convoId, null, () => setLoading(false));
                                }}
                            />
                        }
                    />
                    <ChatLoadingSpinner loading={loading} />
                </>
            );
        };
    }

    makeRestart({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        return () => {
            useEffect(() => {
                (async () => {
                    await client.Widget.Post.UpdateConvoRecord({ IsComplete: true, ConversationId: convoId });
                })();
            }, []);
            return <BotResponse message={node.text} />;
        };
    }

    makeSendEmailFailedFirstAttempt = ({ node, nodeList, client, convoId }: IProgressTheChat) => {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const [loading, setLoading] = useState<boolean>(false);

            return (
                <>
                    <BotResponse
                        message={node.text}
                        button={
                            <>
                                <ResponseButton
                                    text="Send my email"
                                    variant="contained"
                                    onClick={async () => {
                                        setLoading(true);
                                        responseAction(node, child, nodeList, client, convoId, null, () => setLoading(false));
                                    }}
                                />
                                <ResponseButton text="Check your details" variant="contained" onClick={() => openUserDetails()} />
                            </>
                        }
                    />
                    <ChatLoadingSpinner loading={loading} />
                </>
            );
        };
    };

    makeSendFallbackEmail({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const areaId = nodeList[0].areaIdentifier;

        return () => {
            const [disabled, setDisabled] = useState<boolean>(false);
            const [loading, setLoading] = useState<boolean>(false);

            const sendFallbackEmail = async () => {
                const contextProperties = getContextProperties();

                const email = contextProperties[ConvoContextProperties.emailAddress];
                const name = contextProperties[ConvoContextProperties.name];
                const phone = contextProperties[ConvoContextProperties.phoneNumber];
                const locale = contextProperties[ConvoContextProperties.region];

                const response = await client.Widget.Send.FallbackEmail(areaId, email, name, phone, convoId);
                if (response.result) {
                    const completeConvo = assembleEmailRecordData(convoId, areaId, name, email, phone, locale, true);
                    await client.Widget.Post.UpdateConvoRecord(completeConvo);
                }
                return response;
            };
            return (
                <>
                    <BotResponse
                        message={node.text}
                        button={
                            <ResponseButton
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
                        }
                    />
                    <ChatLoadingSpinner loading={loading} />
                </>
            );
        };
    }

    makeEndWithoutEmail({ node, nodeList, client, convoId }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            useEffect(() => {
                setTimeout(async () => {
                    responseAction(node, child, nodeList, client, convoId, null);
                }, 1500);
            }, []);

            return <BotResponse message={node.text} />;
        };
    }
}
