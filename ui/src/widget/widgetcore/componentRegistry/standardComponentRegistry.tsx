import { assembleEmailRecordData, getOrderedChildNodes, getRootNode, MinNumeric } from "../BotResponse/utils/utils";
import React, { useContext, useEffect, useState } from "react";
import { makeStyles } from "@material-ui/core";
import { responseAction } from "@widgetcore/BotResponse/utils/responseAction";
import { ConvoContextProperties } from "./registry";
import { IProgressTheChat, SelectedOption } from "@Palavyr-Types";
import { ResponseButton } from "@widgetcore/BotResponse/ResponseButton";
import { splitValueOptionsByDelimiter } from "@widgetcore/utils/valueOptionSplitter";
import { ChatLoadingSpinner } from "@widgetcore/UserDetailsDialog/ChatLoadingSpinner";
import { FileAsset } from "@widgetcore/BotResponse/FileAsset";
import { NumberFormatValues } from "react-number-format";
import { TextInput } from "@widgetcore/BotResponse/number/TextInput";
import { BotResponse } from "../BotResponse/BotResponse";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { useCallback } from "react";
import { renderNextBotMessage } from "../BotResponse/utils/renderBotMessage";
import { ChoiceList } from "@widgetcore/BotResponse/optionFormats/ChoiceList";
import { MiniContactForm } from "@widgetcore/UserDetailsDialog/CollectDetailsForm";
import { CurrencyTextField } from "@widgetcore/BotResponse/numbers/CurrencyTextField";
import { widgetSelection } from "@common/Analytics/gtag";
import { WidgetPreferencesResource, IntentResource, WidgetNodeResource, FileAssetResource } from "@common/types/api/EntityResources";

const useStyles = makeStyles<{}>((theme: any) => ({
    tableCell: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        borderBottom: "none",
    },
    root: {
        borderBottom: "none",
    },
    textField: (prefs: WidgetPreferencesResource) => ({
        textDecoration: "none",
        color: prefs.chatFontColor,
        borderColor: prefs.chatBubbleColor ? theme.palette.getContrastText(prefs.chatBubbleColor) : "black",
        fontFamily: prefs.fontFamily,
    }),
    textLabel: (prefs: WidgetPreferencesResource) => ({
        color: prefs.chatBubbleColor ? theme.palette.getContrastText(prefs.chatBubbleColor) : "black",
        "&:focus": {
            color: prefs.chatBubbleColor ? theme.palette.getContrastText(prefs.chatBubbleColor) : "black",
        },
        fontFamily: prefs.fontFamily,
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
    inputLabel: (props: WidgetPreferencesResource) => ({
        fontFamily: props.fontFamily,
        color: props.chatFontColor,
        "& .MuiFormLabel-root": {
            fontFamily: props.fontFamily,
            color: props.chatFontColor,
            fontSize: "10pt",
            justifyContent: "center",
        },
        "& .MuiInputBase-input": {
            color: props.chatFontColor,
        },
        "& .MuiInput-underline:before": {
            borderBottomColor: props.chatFontColor, // Semi-transparent underline
        },
        "& .MuiInput-underline:hover:before": {
            borderBottomColor: props.chatFontColor, // Solid underline on hover
        },
        "& .MuiInput-underline:after": {
            borderBottomColor: props.chatFontColor, // Solid underline on focus
        },
    }),
}));

export class StandardComponents {
    public makeSelectOptions({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        return () => {
            const { context, isDemo, preferences } = useContext(WidgetContext);
            const [options, setOptions] = useState<SelectedOption[]>();
            const [disabled, setDisabled] = useState<boolean>(false);
            const [open, setOpen] = useState<boolean>(false);

            const cls = useStyles(preferences);

            const loadIntents = useCallback(async () => {
                var intents = await client.Widget.Get.Intents();
                var options = intents.map((intent: IntentResource) => {
                    return { intentDisplay: intent.intentName, intentId: intent.intentId } as SelectedOption;
                });

                setOptions(options);
            }, []);

            useEffect(() => {
                if (designer) {
                    setOpen(true);
                }
                loadIntents();
            }, [loadIntents]);

            const onChange = async (_: any, newOption: SelectedOption) => {
                if (designer) return;

                const newConversation = await client.Widget.Get.NewConversationHistory({ IntentId: newOption.intentId, Name: context.name, Email: context.emailAddress }, isDemo);
                const nodes = newConversation.conversationNodes;
                const convoId = newConversation.conversationId;
                const rootNode = getRootNode(nodes);
                setDisabled(true);

                // take the name and email adress contexts and send them to the server to check if they are valid. If NOT:

                // renderThatDoesntCheckOutResponse(node, messageResponse, nodeList, client, convoId)
                // renderNextComponent()
                //
                //

                let secretKey = new URLSearchParams(location.search).get("key") as string;
                widgetSelection(secretKey, newOption.intentDisplay, newOption.intentId);
                context.enableReset();
                context.setChatStarted();
                renderNextBotMessage(context, rootNode, nodes, client, convoId);
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
                        <div style={{ marginTop: ".5rem", marginBottom: "5rem", width: "100%" }}>
                            <ChoiceList disabled={disabled} onChange={onChange} setOpen={setOpen} options={options} open={open} />
                        </div>
                    }
                />
            );
        };
    }

    public makeCollectDetails({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const { setConvoId, context, isDemo } = useContext(WidgetContext);

            const [disabled, setDisabled] = useState<boolean>(false);

            const [status, setStatus] = useState<string | null>(null);

            const onFormSubmit = async (e: { preventDefault: () => void }) => {
                if (designer) return;
                e.preventDefault();
                setDisabled(true);
                setConvoId(convoId);
                context.enableDetailsIcon();
                await responseAction(context, node, child, nodeList, client, convoId, isDemo);
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
            const { context, isDemo } = useContext(WidgetContext);

            const callback = useCallback(async () => {
                context.disableReset();
                await responseAction(context, node, child, nodeList, client, convoId, isDemo);
            }, [responseAction, context, context.disableReset]);

            useEffect(() => {
                if (designer) return;
                callback();
            }, []);

            return <BotResponse message={node.text} />;
        };
    }

    public makeMultipleChoiceContinueButtons({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0]; // only one should exist
        const valueOptions = splitValueOptionsByDelimiter(node.valueOptions);

        return () => {
            const [disabled, setDisabled] = useState<boolean>(false);
            const { context, isDemo } = useContext(WidgetContext);

            const onClick = async (valueOption: string) => {
                if (designer) return;
                const response = valueOption;
                await responseAction(context, node, child, nodeList, client, convoId, isDemo, response);
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
            const { context, isDemo } = useContext(WidgetContext);

            const onClick = async (child: WidgetNodeResource) => {
                if (designer) return;
                const response = child.optionPath;
                await responseAction(context, node, child, nodeList, client, convoId, isDemo, response);
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
            const { context, preferences, isDemo } = useContext(WidgetContext);
            const [response, setResponse] = useState<string>("");
            const [disabled, setDisabled] = useState<boolean>(false);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);

            const cls = useStyles(preferences);

            const onChange = (event: React.ChangeEvent<HTMLInputElement>) => {
                const onlyNums = event.target.value.replace(/[^0-9]/g, "");
                setResponse(onlyNums);
                setDisabled(false);
            };

            const onClick = async () => {
                if (designer) return;
                await responseAction(context, node, child, nodeList, client, convoId, isDemo, response);
                setDisabled(true);
                setInputDisabled(true);
            };

            return (
                <BotResponse
                    message={node.text}
                    input={
                        // TODO: Get this happening on the widget side.
                        // <UnitInput
                        //     unitGroup={node.unitGroup}
                        //     unitPrettyName={node.unitPrettyName}
                        //     unitHelperText={node.unitGroup}
                        //     disabled={disabled}
                        //     value={response}
                        //     currencySymbol={node.currencySymbol}
                        //     onCurrencyChange={(values: NumberFormatValues) => {
                        //         if (values.floatValue !== undefined) {
                        //             setResponse(values.floatValue.toString());
                        //         }
                        //     }}
                        //     onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                        //         const val = event.target.value;
                        //         if (val !== "") {
                        //             const result = parseFloat(val);
                        //             if (result) {
                        //                 setResponse(result.toString());
                        //             }
                        //         }
                        //     }}
                        // />
                        // }
                        <TextInput
                            disabled={inputDisabled}
                            className={cls.inputLabel}
                            value={response}
                            label=""
                            type="number"
                            inputPropsClassName={cls.textField}
                            inputLabelPropsClassName={cls.textLabel}
                            onChange={onChange}
                        />
                    }
                    button={<ResponseButton disabled={disabled} onClick={onClick} />}
                />
            );
        };
    }

    makeTakeCurrency({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const { preferences, context, isDemo } = useContext(WidgetContext);
            const cls = useStyles(preferences);
            const [response, setResponse] = useState<number>(0);
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);

            const onClick = async () => {
                if (designer) return;
                await responseAction(context, node, child, nodeList, client, convoId, isDemo, response.toString());
                setDisabled(true);
                setInputDisabled(true);
            };

            return (
                <BotResponse
                    message={node.text}
                    input={
                        <CurrencyTextField
                            // InputProps={{
                            //     className: cls.textField,
                            // }}
                            // InputLabelProps={{
                            //     className: cls.textLabel,
                            // }}
                            className={cls.tableCell}
                            label="Amount"
                            disabled={inputDisabled}
                            // variant="standard"
                            value={response}
                            currencySymbol="$"
                            minimumValue="0"
                            // outputFormat="number"
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

    makeShowResponseFileAsset({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const { context, isDemo } = useContext(WidgetContext);

            let fileAsset: FileAssetResource | null;
            if (designer) {
                fileAsset = {
                    id: 1,
                    fileName: "test.png",
                    link: "https://i.chzbgr.com/full/9591491840/h124EF692/cat-oizzyandthef",
                    fileId: "1234",
                };
            } else {
                fileAsset = context.AppContext.responseFileAsset;
            }

            const callback = useCallback(async () => {
                setTimeout(async () => {
                    await responseAction(context, node, child, nodeList, client, convoId, isDemo);
                }, 2500);
            }, [responseAction]);

            useEffect(() => {
                if (designer) return;
                callback();
            }, [callback]);

            return fileAsset ? <FileAsset fileAsset={fileAsset} /> : <></>;
        };
    }

    makeShowFileAsset({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const { context, isDemo } = useContext(WidgetContext);

            const callback = useCallback(async () => {
                setTimeout(async () => {
                    await responseAction(context, node, child, nodeList, client, convoId, isDemo);
                }, 2500);
            }, [responseAction]);

            useEffect(() => {
                if (designer) return;
                callback();
            }, [callback]);

            return <FileAsset fileAsset={node.fileAssetResource!} />;
        };
    }

    makeTakeText({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const [response, setResponse] = useState<string>("");
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);

            const { preferences, context, isDemo } = useContext(WidgetContext);
            const cls = useStyles(preferences);

            const onClick = async () => {
                if (designer) return;
                setResponse(response);
                await responseAction(context, node, child, nodeList, client, convoId, isDemo, response);
                setDisabled(true);
                setInputDisabled(true);
            };

            return (
                <BotResponse
                    message={node.text}
                    input={
                        <TextInput
                            className={cls.inputLabel}
                            value={response}
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

            const { preferences, context, isDemo } = useContext(WidgetContext);
            const cls = useStyles(preferences);

            const onClick = async () => {
                if (designer) return;
                if (response) {
                    context.setNumIndividuals(response);
                    setDisabled(true);
                    setInputDisabled(true);
                    await responseAction(context, node, child, nodeList, client, convoId, isDemo, response.toString());
                }
            };

            return (
                <BotResponse
                    message={node.text}
                    input={
                        <TextInput
                            className={cls.inputLabel}
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
        const IntentId = nodeList[0].intentId;

        return () => {
            const { context, isDemo } = useContext(WidgetContext);

            const sendEmail = async () => {
                const email = context.AppContext[ConvoContextProperties.emailAddress];
                const name = context.AppContext[ConvoContextProperties.name];
                const phone = context.AppContext[ConvoContextProperties.phoneNumber];
                const locale = context.AppContext[ConvoContextProperties.region];

                let numIndividuals = context.AppContext[ConvoContextProperties.numIndividuals];
                let dynamicResponses = context.AppContext[ConvoContextProperties.dynamicResponses];
                let keyvalues = context.AppContext[ConvoContextProperties.keyValues];

                if (!keyvalues) {
                    keyvalues = [];
                }
                if (!dynamicResponses) {
                    dynamicResponses = [];
                }

                if (!numIndividuals) {
                    numIndividuals = 1;
                }

                const response = await client.Widget.Send.ConfirmationEmail(IntentId, email, name, phone, numIndividuals, dynamicResponses, keyvalues, convoId, isDemo);
                if (response.result) {
                    const completeConvo = assembleEmailRecordData(convoId, IntentId, name, email, phone, locale);
                    if (!isDemo) {
                        await client.Widget.Post.UpdateConvoRecord(completeConvo);
                    }
                    if (response.fileAsset?.link !== null && response.fileAsset?.link !== "" && response.fileAsset?.link !== undefined) {
                        context.setResponseFileAsset(response.fileAsset);
                    }
                }
                return response;
            };
            const [disabled, setDisabled] = useState<boolean>(false);
            const [loading, setLoading] = useState<boolean>(false);

            const onClick = async () => {
                if (designer) return;

                setDisabled(true);
                setLoading(true);
                const response = await sendEmail();
                const child = nodeList.filter((x: WidgetNodeResource) => x.nodeId === response.nextNodeId)[0];
                await responseAction(context, node, child, nodeList, client, convoId, isDemo, null, () => setLoading(false));
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
            const { isDemo } = useContext(WidgetContext);

            const callback = useCallback(async () => {
                setTimeout(async () => {
                    await client.Widget.Post.UpdateConvoRecord({ IsComplete: true, ConversationId: convoId });
                }, 1500);
            }, [responseAction]);

            useEffect(() => {
                if (designer) return;
                if (isDemo) return;
                callback();
            }, [callback]);
            return <BotResponse message={node.text} />;
        };
    }

    makeSendEmailFailedFirstAttempt = ({ node, nodeList, client, convoId, designer }: IProgressTheChat) => {
        const child = getOrderedChildNodes(node.nodeChildrenString, nodeList)[0];

        return () => {
            const [loading, setLoading] = useState<boolean>(false);
            const { context, isDemo } = useContext(WidgetContext);

            const onClick = async () => {
                if (designer) return;
                setLoading(true);
                await responseAction(context, node, child, nodeList, client, convoId, isDemo, null, () => setLoading(false));
            };
            return (
                <>
                    <BotResponse
                        message={node.text}
                        button={
                            <>
                                <ResponseButton text="Send my email" variant="contained" onClick={onClick} />
                                <ResponseButton text="Check your details" variant="contained" onClick={() => context.openUserDetails()} />
                            </>
                        }
                    />
                    <ChatLoadingSpinner loading={loading} />
                </>
            );
        };
    };

    makeSendFallbackEmail({ node, nodeList, client, convoId, designer }: IProgressTheChat): React.ElementType<{}> {
        const IntentId = nodeList[0].intentId;

        return () => {
            const [disabled, setDisabled] = useState<boolean>(false);
            const [loading, setLoading] = useState<boolean>(false);
            const { context, isDemo } = useContext(WidgetContext);

            const sendFallbackEmail = async () => {
                const email = context.AppContext[ConvoContextProperties.emailAddress];
                const name = context.AppContext[ConvoContextProperties.name];
                const phone = context.AppContext[ConvoContextProperties.phoneNumber];
                const locale = context.AppContext[ConvoContextProperties.region];

                const response = await client.Widget.Send.FallbackEmail(IntentId, email, name, phone, convoId, isDemo);
                if (response.result) {
                    if (!isDemo) {
                        const completeConvo = assembleEmailRecordData(convoId, IntentId, name, email, phone, locale, true);
                        await client.Widget.Post.UpdateConvoRecord(completeConvo);
                    }
                }
                return response;
            };

            const onClick = async () => {
                if (designer) return;
                setLoading(true);
                const response = await sendFallbackEmail();
                const child = nodeList.filter((x: WidgetNodeResource) => x.nodeId === response.nextNodeId)[0];
                await responseAction(context, node, child, nodeList, client, convoId, isDemo, null, () => setLoading(false));
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
            const { context, isDemo } = useContext(WidgetContext);

            const callback = useCallback(async () => {
                setTimeout(async () => {
                    await responseAction(context, node, child, nodeList, client, convoId, isDemo);
                }, 1500);
            }, [responseAction]);

            useEffect(() => {
                if (designer) return;
                callback();
            }, [callback]);

            return <BotResponse message={node.text} />;
        };
    }
}
