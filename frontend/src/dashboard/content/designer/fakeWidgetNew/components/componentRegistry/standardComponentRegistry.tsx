import { CurrencyTextField } from "@common/components/borrowed/CurrentTextField";
import { makeStyles } from "@material-ui/core";
import { WidgetPreferences, SelectedOption, WidgetNodeResource } from "@Palavyr-Types";
import React, { useState, useCallback, useEffect, useContext } from "react";
import { NumberFormatValues } from "react-number-format";
import { BotResponse } from "../BotResponse/BotResponse";
import { CustomImage } from "../BotResponse/CustomImage";
import { TextInput } from "../BotResponse/number/TextInput";
import { ChoiceList } from "../BotResponse/optionFormats/ChoiceList";
import { ResponseButton } from "../BotResponse/ResponseButton";
import { WidgetContext } from "../context/WidgetContext";
import { ChatLoadingSpinner } from "../UserDetailsDialog/ChatLoadingSpinner";
import { MiniContactForm } from "../UserDetailsDialog/CollectDetailsForm";

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
        width: "80%",
        borderColor: prefs.chatBubbleColor ? theme.palette.getContrastText(prefs.chatBubbleColor) : "black",
    }),
    textLabel: (prefs: WidgetPreferences) => ({
        width: "60ch",
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
    public makeSelectOptions(text: string): React.ElementType<{}> {
        return () => {
            const [options, setOptions] = useState<Array<SelectedOption>>();
            const [disabled, setDisabled] = useState<boolean>(false);
            const [open, setOpen] = useState<boolean>(false);

            const loadAreas = useCallback(async () => {
                setOptions([{ areaDisplay: "What does it cost?", areaId: "1" }, { areaDisplay: "How do I change my colors?", areaId: "2" }, { areaDisplay: "What is Palavyr?", areaId: "3" }]);
            }, []);

            useEffect(() => {
                loadAreas();
            }, [loadAreas]);

            const onChange = async (_: any, newOption: SelectedOption) => {};

            return (
                <div style={{ width: "100%" }}>
                    <BotResponse
                        message={text}
                        input={
                            <div style={{ marginTop: "2rem", marginBottom: "2rem", width: "100%" }}>
                                <ChoiceList disabled={disabled} onChange={onChange} setOpen={setOpen} options={options} open={open} />
                            </div>
                        }
                    />
                </div>
            );
        };
    }

    public makeCollectDetails(text: string): React.ElementType<{}> {
        return () => {
            const [disabled, setDisabled] = useState<boolean>(true);

            const [status, setStatus] = useState<string | null>(null);
            const [detailsSet, setDetailsSet] = useState<boolean>(false);

            const onFormSubmit = (e: { preventDefault: () => void }) => {
                e.preventDefault();
            };

            return (
                <BotResponse
                    message={text}
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

    public makeProvideInfo(text: string): React.ElementType<{}> {
        return () => {
            return <BotResponse message={text} />;
        };
    }

    public makeMultipleChoiceContinueButtons(text: string): React.ElementType<{}> {
        return () => {
            const [disabled, setDisabled] = useState<boolean>(false);
            return (
                <BotResponse
                    message={text}
                    buttons={
                        <div style={{ display: "flex", flexDirection: "row", width: "100%", justifyContent: "flex-start" }}>
                            {["One", "Two", "Three"].map((valueOption: string, index: number) => {
                                return <ResponseButton disabled={disabled} key={valueOption + "-" + index} text={valueOption} />;
                            })}
                        </div>
                    }
                />
            );
        };
    }

    public makeMultipleChoiceAsPathButtons(text: string): React.ElementType<{}> {
        return () => {
            const [disabled, setDisabled] = useState<boolean>(false);
            const children: Partial<WidgetNodeResource>[] = [{ nodeId: "1", optionPath: "Option 1" }, { nodeId: "2", optionPath: "Option 2" }, { nodeId: "3", optionPath: "Option 3" }];
            return (
                <BotResponse
                    message={text}
                    buttons={
                        <>
                            {children.map((child: WidgetNodeResource) => {
                                return <>{child.optionPath && <ResponseButton disabled={disabled} key={child.nodeId} text={child.optionPath} onClick={() => {}} />}</>;
                            })}
                        </>
                    }
                />
            );
        };
    }

    public makeTakeNumber(text: string): React.ElementType<{}> {
        return () => {
            const [disabled, setDisabled] = useState<boolean>(true);

            const { preferences } = useContext(WidgetContext);
            const cls = useStyles(preferences);
            return (
                <BotResponse
                    message={text}
                    input={
                        <TextInput
                            label=""
                            inputPropsClassName={cls.textField}
                            inputLabelPropsClassName={cls.textLabel}
                            onChange={event => {
                                setDisabled(false);
                            }}
                        />
                    }
                    button={<ResponseButton disabled={true} />}
                />
            );
        };
    }

    makeTakeCurrency(text: string): React.ElementType<{}> {
        return () => {
            const { preferences } = useContext(WidgetContext);
            const cls = useStyles(preferences);
            const [response, setResponse] = useState<number>(0);
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);
            return (
                <BotResponse
                    message={text}
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
                    button={<ResponseButton disabled={disabled} onClick={() => {}} />}
                />
            );
        };
    }

    makeShowImage(text: string): React.ElementType<{}> {
        return () => {
            const [link, setLink] = useState<string>("");
            return <CustomImage imageLink={link} />;
        };
    }

    makeTakeText(text: string): React.ElementType<{}> {
        return () => {
            const [response, setResponse] = useState<string>("");
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);

            const { preferences } = useContext(WidgetContext);
            const cls = useStyles(preferences);
            return (
                <BotResponse
                    message={text}
                    input={
                        <TextInput
                            fullWidth
                            inputPropsClassName={cls.textField}
                            inputLabelPropsClassName={cls.textLabel}
                            disabled={inputDisabled}
                            onChange={event => {
                                setResponse(event.target.value);
                                setDisabled(false);
                            }}
                        />
                    }
                    button={<ResponseButton disabled={disabled || response === ""} text="Submit" />}
                />
            );
        };
    }

    makeTakeNumberIndividuals(text: string): React.ElementType<{}> {
        return () => {
            const [response, setResponse] = useState<number | null>(null);
            const [disabled, setDisabled] = useState<boolean>(true);
            const [inputDisabled, setInputDisabled] = useState<boolean>(false);
            const { preferences } = useContext(WidgetContext);
            const cls = useStyles(preferences);
            return (
                <BotResponse
                    message={text}
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
                                if (intValue < 0) {
                                    setResponse(0);
                                } else {
                                    setResponse(intValue);
                                }
                            }}
                        />
                    }
                    button={<ResponseButton disabled={disabled} />}
                />
            );
        };
    }

    makeSendEmail(text: string): React.ElementType<{}> {
        return () => {
            const [disabled, setDisabled] = useState<boolean>(false);
            const [loading, setLoading] = useState<boolean>(false);
            return (
                <>
                    <BotResponse message={text} button={<ResponseButton text="Send my email" variant="contained" disabled={disabled} />} />
                    <ChatLoadingSpinner loading={loading} />
                </>
            );
        };
    }

    makeRestart(text: string): React.ElementType<{}> {
        return () => {
            return <BotResponse message={text} />;
        };
    }

    makeSendEmailFailedFirstAttempt = (text: string) => {
        return () => {
            const [loading, setLoading] = useState<boolean>(false);

            return (
                <>
                    <BotResponse
                        message={text}
                        button={
                            <>
                                <ResponseButton text="Send my email" variant="contained" />
                                <ResponseButton text="Check your details" variant="contained" />
                            </>
                        }
                    />
                    <ChatLoadingSpinner loading={loading} />
                </>
            );
        };
    };

    makeSendFallbackEmail(text: string): React.ElementType<{}> {
        return () => {
            const [disabled, setDisabled] = useState<boolean>(false);
            const [loading, setLoading] = useState<boolean>(false);

            return (
                <>
                    <BotResponse message={text} button={<ResponseButton text="Send my email" variant="contained" disabled={disabled} />} />
                    <ChatLoadingSpinner loading={loading} />
                </>
            );
        };
    }

    makeEndWithoutEmail(text: string): React.ElementType<{}> {
        return () => {
            return <BotResponse message={text} />;
        };
    }
}
