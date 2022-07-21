import React, { useState, useCallback, useEffect, useContext } from "react";
import { Divider } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { Settings, ResponseVariable } from "@Palavyr-Types";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { OsTypeToggle } from "../../areaSettings/enableIntents/OsTypeToggle";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { EmailConfigurationComponent } from "./EmailConfigurationComponent";

export const EmailConfiguration = () => {
    const { repository } = useContext(DashboardContext);

    const { intentId } = useParams<{ intentId: string }>();

    const [loaded, setLoaded] = useState<boolean>(false);
    const [settings, setSettings] = useState<Partial<Settings>>({ useIntentFallbackEmail: false });
    const [variableDetails, setVariableDetails] = useState<ResponseVariable[]>();

    const [useIntentEmail, setUseIntentEmail] = useState<boolean>(false);
    const [useIntentFallbackEmail, setuseIntentFallbackEmail] = useState<boolean>(false);

    const onuseIntentFallbackEmailToggle = async () => {
        const updatedUsIntentFallback = await repository.Intent.ToggleuseIntentFallbackEmail(!useIntentEmail, intentId);
        setUseIntentEmail(updatedUsIntentFallback);
        setuseIntentFallbackEmail(!useIntentFallbackEmail);
    };

    const loadVariableDetails = useCallback(async () => {
        const variableDetails = await repository.Configuration.Email.GetVariableDetails();
        setVariableDetails(variableDetails);
        return () => {
            setLoaded(false);
        };
    }, []);

    const loadSettings = useCallback(async () => {
        const intents = await repository.Intent.GetAllIntents();
        const intentData = intents.filter((x) => x.intentId === intentId)[0];
        setSettings({
            ...settings,
            useIntentFallbackEmail: intentData.useIntentFallbackEmail,
        });
        setuseIntentFallbackEmail(intentData.useIntentFallbackEmail);
    }, []);

    useEffect(() => {
        loadVariableDetails();
        loadSettings();
        return () => {
            setLoaded(false);
        };
    }, [loadVariableDetails, loadSettings]);

    return (
        <>
            <HeaderStrip title="Email Response" subtitle="Use this editor to create an HTML email template that will be sent as the email response for this intent." />
            {useIntentFallbackEmail !== null && (
                <OsTypeToggle controlledState={useIntentFallbackEmail} onChange={onuseIntentFallbackEmailToggle} enabledLabel="Use Intent Fallback Email" disabledLabel="Use General Fallback Email" />
            )}
            {variableDetails && (
                <EmailConfigurationComponent
                    variableDetails={variableDetails}
                    saveEmailTemplate={async (emailTemplate: string) => await repository.Configuration.Email.SaveIntentEmailTemplate(intentId, emailTemplate)}
                    saveEmailSubject={async (subject: string) => await repository.Configuration.Email.SaveIntentSubject(intentId, subject)}
                    getCurrentEmailSubject={async () => await repository.Configuration.Email.GetIntentSubject(intentId)}
                    getCurrentEmailTemplate={async () => await repository.Configuration.Email.GetIntentEmailTemplate(intentId)}
                />
            )}
            <Divider />
            {useIntentFallbackEmail && (
                <>
                    <HeaderStrip
                        title="Fallback Email Response"
                        subtitle="Use this editor to create a fallback email response that is specific to this area. For example, this email is sent if a 'Too Complicated' node is encountered during the chat."
                    />
                    {variableDetails && (
                        <EmailConfigurationComponent
                            variableDetails={variableDetails}
                            saveEmailTemplate={async (emailTemplate: string) => await repository.Configuration.Email.SaveIntentFallbackEmailTemplate(intentId, emailTemplate)}
                            saveEmailSubject={async (emailSubject: string) => await repository.Configuration.Email.SaveIntentFallbackSubject(intentId, emailSubject)}
                            getCurrentEmailSubject={async () => await repository.Configuration.Email.GetIntentFallbackSubject(intentId)}
                            getCurrentEmailTemplate={async () => await repository.Configuration.Email.GetIntentFallbackEmailTemplate(intentId)}
                        />
                    )}
                </>
            )}
        </>
    );
};
