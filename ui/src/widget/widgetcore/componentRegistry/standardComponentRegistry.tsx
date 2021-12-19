import { assembleEmailRecordData, getOrderedChildNodes, getRootNode, MinNumeric, parseNumericResponse } from "../BotResponse/utils/utils";
import React, { useContext, useEffect, useState } from "react";
import { makeStyles } from "@material-ui/core";
import { responseAction } from "@widgetcore/BotResponse/utils/responseAction";
import { ConvoContextProperties } from "./registry";
import { AreaTable, IProgressTheChat, SelectedOption, WidgetNodeResource, WidgetPreferences } from "@Palavyr-Types";
import { setNumIndividualsContext, getContextProperties, openUserDetails, getNameContext, getEmailAddressContext, setPdfLink } from "@store-dispatcher";
import { ResponseButton } from "@widgetcore/BotResponse/ResponseButton";
import { splitValueOptionsByDelimiter } from "@widgetcore/utils/valueOptionSplitter";
import { ChatLoadingSpinner } from "@widgetcore/UserDetailsDialog/ChatLoadingSpinner";
import { CustomImage } from "@widgetcore/BotResponse/CustomImage";
import { NumberFormatValues } from "react-number-format";
import { TextInput } from "@widgetcore/BotResponse/number/TextInput";
import { BotResponse } from "../BotResponse/BotResponse";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { useCallback } from "react";
import { renderNextComponent } from "../BotResponse/utils/renderNextComponent";
import { ChoiceList } from "@widgetcore/BotResponse/optionFormats/ChoiceList";
import { MiniContactForm } from "@widgetcore/UserDetailsDialog/CollectDetailsForm";
import { CurrencyTextField } from "@widgetcore/BotResponse/numbers/CurrencyTextField";

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
    public makeSelectOptions({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
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
                if (designer) {
                    setOpen(true);
                }
                loadAreas();
            }, [loadAreas]);

            const onChange = async (_: any, newOption: SelectedOption) => {
                if (designer) return;

                const newConversation = await client.Widget.Get.NewConversation(newOption.areaId, { Name: getNameContext(), Email: getEmailAddressContext() });
                const nodes = newConversation.conversationNodes;
                const convoId = newConversation.conversationId;
                const rootNode = getRootNode(nodes);
                setDisabled(true);

                // take the name and email adress contexts and send them to the server to check if they are valid. If NOT:

                // renderThatDoesntCheckOutResponse(node, messageResponse, nodeList, client, convoId)
                // renderNextComponent()
                //
                //

                renderNextComponent(rootNode, nodes, client, convoId);
            };

            // const renderThatDoesntCheckOUtResponse = (node, messageResponse, nodeList, client, convoId) => {

            //     useEffect(() => {
            //         setTimeout(() => {
            //             renderNextComponent(node, nodeList, client, convoId)
            //         }, 1500);

            //     })
            //     return <BotResponse message={`That doesn't quite check out. Could you try that again?${messageResponse}`} />
            // }

            return (
                <BotResponse
                    message={node.text}
                    input={
                        <div style={{ marginTop: ".5rem", marginBottom: "2rem", width: "100%" }}>
                            <ChoiceList disabled={disabled} onChange={onChange} setOpen={setOpen} options={options} open={designer ? designer : open} />
                        </div>
                    }
                />
            );
        };
    }

    public makeCollectDetails({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const { setChatStarted, setConvoId } = useContext(WidgetContext);

            const [disabled, setDisabled] = useState<boolean>(false);

            const [status, setStatus] = useState<string | null>(null);
            const [detailsSet, setDetailsSet] = useState<boolean>(false);

            const onFormSubmit = (e: { preventDefault: () => void }) => {
                if (designer) return;
                e.preventDefault();
                setDisabled(true);
                setChatStarted(true);
                setConvoId(convoId);
                responseAction(node, child, nodeList, client, convoId, null);
            };

            useEffect(() => {
                if (designer) {
                    setDisabled(true);
                }
            }, []);

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

    public makeProvideInfo({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];
        return () => {
            useEffect(() => {
                if (designer) return;
                responseAction(node, child, nodeList, client, convoId, null);
            }, []);

            return <BotResponse message={node.text} />;
        };
    }

    public makeProvideInfoWithPdfLink({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];
        const context = getContextProperties();
        const pdfLink = designer ? " " : context.pdfLink;

        return () => {
            useEffect(() => {
                if (designer) return;
                responseAction(node, child, nodeList, client, convoId, null);
            }, []);

            return <BotResponse message={node.text} pdfLink={pdfLink} />;
        };
    }

    public makeMultipleChoiceContinueButtons({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0]; // only one should exist
        const valueOptions = splitValueOptionsByDelimiter(node.valueOptions);

        return () => {
            const [disabled, setDisabled] = useState<boolean>(false);

            const onClick = (valueOption: string) => {
                if (designer) return;
                const response = valueOption;
                responseAction(node, child, nodeList, client, convoId, response);
                setDisabled(true);
            };

            return (
                <BotResponse
                    message={node.text}
                    buttons={
                        <div style={{ display: "flex", flexDirection: "row", width: "100%", justifyContent: "flex-start" }}>
                            {valueOptions.map((valueOption: string, index: number) => {
                                return <ResponseButton disabled={disabled} key={valueOption + "-" + index} text={valueOption} onClick={() => onClick(valueOption)} />;
                            })}
                        </div>
                    }
                />
            );
        };
    }

    public makeMultipleChoiceAsPathButtons({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const children = getOrderedChildNodes(node.nodeChildrenString, nodeList);

        return () => {
            const [disabled, setDisabled] = useState<boolean>(false);

            const onClick = (child: WidgetNodeResource) => {
                if (designer) return;
                const response = child.optionPath;
                responseAction(node, child, nodeList, client, convoId, response);
                setDisabled(true);
            };

            return (
                <BotResponse
                    message={node.text}
                    buttons={
                        <>
                            {children.map((child: WidgetNodeResource) => {
                                return <>{child.optionPath && <ResponseButton disabled={disabled} key={child.nodeId} text={child.optionPath} onClick={() => onClick(child)} />}</>;
                            })}
                        </>
                    }
                />
            );
        };
    }

    public makeTakeNumber({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        let child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const [response, setResponse] = useState<string>("");
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);

            const { preferences } = useContext(WidgetContext);
            const cls = useStyles(preferences);

            const onChange = (event: React.ChangeEvent<HTMLInputElement>) => {
                setResponse(parseNumericResponse(event.target.value));
                setDisabled(false);
            };

            const onClick = async () => {
                if (designer) return;
                responseAction(node, child, nodeList, client, convoId, response);
                setDisabled(true);
                setInputDisabled(true);
            };

            return (
                <BotResponse
                    message={node.text}
                    input={<TextInput label="" type="number" inputPropsClassName={cls.textField} inputLabelPropsClassName={cls.textLabel} onChange={onChange} />}
                    button={<ResponseButton disabled={disabled} onClick={onClick} />}
                />
            );
        };
    }

    makeTakeCurrency({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const { preferences } = useContext(WidgetContext);
            const cls = useStyles(preferences);
            const [response, setResponse] = useState<number>(0);
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);

            const onClick = () => {
                if (designer) return;
                responseAction(node, child, nodeList, client, convoId, response.toString());
                setDisabled(true);
                setInputDisabled(true);
            };

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
                    button={<ResponseButton disabled={disabled} onClick={onClick} />}
                />
            );
        };
    }

    makeShowImage({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const [loaded, setLoaded] = useState<boolean>(false);
            const [link, setLink] = useState<string>("");

            useEffect(() => {
                (async () => {
                    if (designer) return;
                    const presignedUrl = await client.Widget.Get.NodeImage(node.nodeId);
                    setLink(presignedUrl);
                    setLoaded(false);
                })();

                setTimeout(() => {
                    if (designer) return;
                    responseAction(node, child, nodeList, client, convoId, null);
                }, 2500);
            }, []);

            return <CustomImage imageLink={link} />;
        };
    }

    makeTakeText({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const [response, setResponse] = useState<string>("");
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);

            const { preferences } = useContext(WidgetContext);
            const cls = useStyles(preferences);

            const onClick = () => {
                if (designer) return;
                setResponse(response);
                responseAction(node, child, nodeList, client, convoId, response);
                setDisabled(true);
                setInputDisabled(true);
            };

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
                    button={<ResponseButton disabled={disabled || response === ""} text="Submit" onClick={onClick} />}
                />
            );
        };
    }

    makeTakeNumberIndividuals({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const [response, setResponse] = useState<number | null>(null);
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);

            const { preferences } = useContext(WidgetContext);
            const cls = useStyles(preferences);

            const onClick = () => {
                if (designer) return;
                if (response) {
                    responseAction(node, child, nodeList, client, convoId, response.toString());
                    setNumIndividualsContext(response);
                    setDisabled(true);
                    setInputDisabled(true);
                }
            };

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
                    button={<ResponseButton disabled={disabled} onClick={onClick} />}
                />
            );
        };
    }

    makeSendEmail({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
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
                if (response.pdfLink !== null && response.pdfLink !== "" && response.pdfLink !== undefined) {
                    setPdfLink(response.pdfLink);
                }
            }
            return response;
        };

        return () => {
            const [disabled, setDisabled] = useState<boolean>(false);
            const [loading, setLoading] = useState<boolean>(false);

            const onClick = async () => {
                if (designer) return;

                setDisabled(true);
                setLoading(true);
                const response = await sendEmail();
                const child = nodeList.filter((x: WidgetNodeResource) => x.nodeId === response.nextNodeId)[0];
                responseAction(node, child, nodeList, client, convoId, null, () => setLoading(false));
            };
            return (
                <>
                    <BotResponse message={node.text} button={<ResponseButton text="Send my email" variant="contained" disabled={disabled} onClick={onClick} />} />
                    <ChatLoadingSpinner loading={loading} />
                </>
            );
        };
    }

    makeRestart({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        return () => {
            useEffect(() => {
                (async () => {
                    if (designer) return;
                    await client.Widget.Post.UpdateConvoRecord({ IsComplete: true, ConversationId: convoId });
                })();
            }, []);
            return <BotResponse message={node.text} />;
        };
    }

    makeSendEmailFailedFirstAttempt = ({ node, nodeList, client, convoId, designer }: IProgressTheChat) => {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const [loading, setLoading] = useState<boolean>(false);
            const onClick = async () => {
                if (designer) return;
                setLoading(true);
                responseAction(node, child, nodeList, client, convoId, null, () => setLoading(false));
            };
            return (
                <>
                    <BotResponse
                        message={node.text}
                        button={
                            <>
                                <ResponseButton text="Send my email" variant="contained" onClick={onClick} />
                                <ResponseButton text="Check your details" variant="contained" onClick={() => openUserDetails()} />
                            </>
                        }
                    />
                    <ChatLoadingSpinner loading={loading} />
                </>
            );
        };
    };

    makeSendFallbackEmail({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
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

            const onClick = async () => {
                if (designer) return;
                setLoading(true);
                const response = await sendFallbackEmail();
                const child = nodeList.filter((x: WidgetNodeResource) => x.nodeId === response.nextNodeId)[0];
                responseAction(node, child, nodeList, client, convoId, null, () => setLoading(false));
                setDisabled(true);
            };

            return (
                <>
                    <BotResponse message={node.text} button={<ResponseButton text="Send my email" variant="contained" disabled={disabled} onClick={onClick} />} />
                    <ChatLoadingSpinner loading={loading} />
                </>
            );
        };
    }

    makeEndWithoutEmail({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            useEffect(() => {
                setTimeout(async () => {
                    if (designer) return;
                    responseAction(node, child, nodeList, client, convoId, null);
                }, 1500);
            }, []);

            return <BotResponse message={node.text} />;
        };
    }
}
