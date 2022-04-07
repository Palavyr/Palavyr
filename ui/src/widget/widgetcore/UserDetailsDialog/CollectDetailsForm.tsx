import { Button, Dialog, DialogContent, makeStyles, Typography } from "@material-ui/core";
import React, { useState } from "react";
import { SetStateAction } from "react";
import { Dispatch } from "react";
import CheckCircleOutlineIcon from "@material-ui/icons/CheckCircleOutline";
import { useLocation } from "react-router-dom";
import { useEffect } from "react";
import { UserDetailsTitle } from "./UserDetailsTitle";
import { NameForm } from "./FormInputs/NameForm";
import { EmailForm } from "./FormInputs/EmailForm";
import { LocaleSelector } from "./FormInputs/LocaleSelector";
import { PhoneForm } from "./FormInputs/PhoneForm";
import { LocaleMap, LocaleResource, SetState, WidgetPreferences } from "@Palavyr-Types";
import { INVALID_PHONE, INVALID_EMAIL, INVALID_NAME } from "./UserDetailsCheck";
import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { useContext } from "react";

export interface CollectDetailsFormProps {
    setKickoff: SetState<boolean>;
}

export interface BaseFormProps {
    status: string | null;
    setStatus: Dispatch<SetStateAction<string>>;
}

const useStyles = makeStyles(theme => ({
    baseDialogCollectionForm: {
        position: "absolute",
    },
    dialogBackgroundCollectionForm: (props: WidgetPreferences) => ({
        border: "none",
        boxShadow: "none",
        shadow: "none",
        backgroundColor: props.chatBubbleColor,
    }),
    dialogPaperCollectionForm: (props: WidgetPreferences) => ({
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        paddingBottom: theme.spacing(3),
        maxWidth: 420,
        backgroundColor: props.chatBubbleColor,
        border: "none",
        boxShadow: "none",
        shadow: "none",
    }),
    dialogPaperScrollPaperCollectionForm: {
        maxHeight: "none",
        border: "none",
        boxShadow: "none",
        shadow: "none",
    },
    dialogContentCollectionForm: {
        paddingTop: 0,
        paddingBottom: 0,
        border: "none",
        boxShadow: "none",
        shadow: "none",
    },
    buttonCollectionForm: (props: WidgetPreferences) => ({
        margin: "0.5rem",

        textAlign: "center",
        marginTop: "2.0rem",
        border: "none",
        boxShadow: "none",
        shadow: "none",
        color: props.buttonFontColor,
        backgroundColor: props.buttonColor,
    }),
    backgropPropsClassName: (props: WidgetPreferences) => ({
        backgroundColor: props.chatBubbleColor,
    }),
}));

export const CollectDetailsForm = ({ setKickoff }: CollectDetailsFormProps) => {
    const secretKey = new URLSearchParams(useLocation().search).get("key");
    const client = new PalavyrWidgetRepository(secretKey);
    const [options, setOptions] = useState<LocaleMap>([]);
    const [phonePattern, setphonePattern] = useState<string>("");
    const [detailsSet, setDetailsSet] = useState<boolean>(false);

    const { chatStarted, setChatStarted, convoId, context, preferences, isDemo } = useContext(WidgetContext);

    useEffect(() => {
        (async () => {
            const { currentLocale: locale, localeMap } = await client.Widget.Get.Locale();
            setphonePattern(locale.phoneFormat);
            setOptions(localeMap);
            context.setRegion(locale.name);
        })();
    }, []);

    const cls = useStyles(preferences);
    const [status, setStatus] = useState<string | null>(null);

    const onChange = (_: any, newOption: LocaleResource) => {
        setphonePattern(newOption.phoneFormat);
        context.setRegion(newOption.name);
    };

    const onFormSubmit = async (e: { preventDefault: () => void }) => {
        e.preventDefault();
        setKickoff(true);
        setChatStarted(true);

        if (convoId && !isDemo) {
            await client.Widget.Post.UpdateConvoRecord({ Name: context.name, Email: context.emailAddress, PhoneNumber: context.phoneNumber, Locale: context.region, ConversationId: convoId });
        }
        context.closeUserDetails();
    };

    const formProps = {
        status,
        setStatus,
    };

    return (
        <>
            <Dialog
                open={context.userDetailsVisible && chatStarted}
                className={cls.baseDialogCollectionForm}
                classes={{
                    root: cls.dialogBackgroundCollectionForm,
                    paper: cls.dialogPaperCollectionForm,
                    paperScrollPaper: cls.dialogPaperScrollPaperCollectionForm,
                    scrollPaper: cls.dialogPaperScrollPaperCollectionForm,
                }}
                BackdropProps={{ className: cls.backgropPropsClassName }}
                onClose={_ => null}
                hideBackdrop={false}
                disableEscapeKeyDown
            >
                <UserDetailsTitle title="Update your Contact Details" />
                <DialogContent className={cls.dialogContentCollectionForm}>
                    <ContactForm
                        disabled={false}
                        localeOptions={options}
                        onFormSubmit={onFormSubmit}
                        formProps={{ ...formProps }}
                        setDetailsSet={setDetailsSet}
                        phonePattern={phonePattern}
                        onChange={onChange}
                        detailsSet={detailsSet}
                        submitButton={
                            <Button
                                disabled={status === INVALID_PHONE || status === INVALID_EMAIL || status === INVALID_NAME}
                                className={cls.buttonCollectionForm}
                                endIcon={detailsSet && <CheckCircleOutlineIcon />}
                                type="submit"
                            >
                                <Typography variant="h5">{chatStarted ? "Continue" : "Begin"}</Typography>
                            </Button>
                        }
                    />
                </DialogContent>
            </Dialog>
        </>
    );
};

export interface ContactFormProps {
    onFormSubmit(e: { preventDefault: () => void }): void;
    formProps: any;
    setDetailsSet: any;
    phonePattern: any;
    onChange: any;
    detailsSet: boolean;
    localeOptions: any;
    submitButton: React.ReactNode;
    disabled: boolean;
}

export const ContactForm = ({ disabled, onFormSubmit, submitButton, localeOptions, formProps, setDetailsSet, phonePattern, onChange, detailsSet }: ContactFormProps) => {
    return (
        <form onSubmit={onFormSubmit}>
            <NameForm {...formProps} disabled={disabled} />
            <EmailForm {...formProps} setDetailsSet={setDetailsSet} disabled={disabled} />
            <PhoneForm {...formProps} phonePattern={phonePattern} disabled={disabled} />
            <LocaleSelector options={localeOptions} onChange={onChange} disabled={disabled} />
            <div style={{ display: "flex", justifyContent: "center" }}>{submitButton}</div>
        </form>
    );
};

export interface MiniContactFormProps {
    onFormSubmit(e: { preventDefault: () => void }): void;
    formProps: any;
    setDetailsSet: SetState<boolean>;
    submitButton: React.ReactNode;
    disabled: boolean;
}
export const MiniContactForm = ({ disabled, onFormSubmit, setDetailsSet, submitButton, formProps }: MiniContactFormProps) => {
    return (
        <form onSubmit={onFormSubmit}>
            <NameForm {...formProps} disabled={disabled} />
            <EmailForm {...formProps} setDetailsSet={setDetailsSet} disabled={disabled} />
            <div style={{ display: "flex", justifyContent: "right" }}>{submitButton}</div>
        </form>
    );
};
