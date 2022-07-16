import React, { useState, useCallback, useEffect, useContext } from "react";
import { Divider } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { Settings, VariableDetail } from "@Palavyr-Types";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { OsTypeToggle } from "../../areaSettings/enableAreas/OsTypeToggle";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { EmailConfigurationComponent } from "./EmailConfigurationComponent";

export const EmailConfiguration = () => {
    const { repository } = useContext(DashboardContext);

    const { intentId } = useParams<{ intentId: string }>();

    const [loaded, setLoaded] = useState<boolean>(false);
    const [settings, setSettings] = useState<Partial<Settings>>({ useAreaFallbackEmail: false });
    const [variableDetails, setVariableDetails] = useState<VariableDetail[]>();

    const [useAreaEmail, setUseAreaEmail] = useState<boolean>(false);
    const [useAreaFallbackEmail, setUseAreaFallbackEmail] = useState<boolean>(false);

    const onUseAreaFallbackEmailToggle = async () => {
        const updatedUsAreaFallback = await repository.Intent.ToggleUseAreaFallbackEmail(!useAreaEmail, intentId);
        setUseAreaEmail(updatedUsAreaFallback);
        setUseAreaFallbackEmail(!useAreaFallbackEmail);
    };

    const loadVariableDetails = useCallback(async () => {
        const variableDetails = await repository.Configuration.Email.GetVariableDetails();
        setVariableDetails(variableDetails);
        return () => {
            setLoaded(false);
        };
    }, []);

    const loadSettings = useCallback(async () => {
        const areas = await repository.Intent.GetAllIntents();
        const areaData = areas.filter((x) => x.intentId === intentId)[0];
        setSettings({
            ...settings,
            useAreaFallbackEmail: areaData.useAreaFallbackEmail,
        });
        setUseAreaFallbackEmail(areaData.useAreaFallbackEmail);
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
            {useAreaFallbackEmail !== null && (
                <OsTypeToggle controlledState={useAreaFallbackEmail} onChange={onUseAreaFallbackEmailToggle} enabledLabel="Use Intent Fallback Email" disabledLabel="Use General Fallback Email" />
            )}
            {variableDetails && (
                <EmailConfigurationComponent
                    variableDetails={variableDetails}
                    saveEmailTemplate={async (emailTemplate: string) => await repository.Configuration.Email.SaveAreaEmailTemplate(intentId, emailTemplate)}
                    saveEmailSubject={async (subject: string) => await repository.Configuration.Email.SaveAreaSubject(intentId, subject)}
                    getCurrentEmailSubject={async () => await repository.Configuration.Email.GetAreaSubject(intentId)}
                    getCurrentEmailTemplate={async () => await repository.Configuration.Email.GetAreaEmailTemplate(intentId)}
                />
            )}
            <Divider />
            {useAreaFallbackEmail && (
                <>
                    <HeaderStrip
                        title="Fallback Email Response"
                        subtitle="Use this editor to create a fallback email response that is specific to this area. For example, this email is sent if a 'Too Complicated' node is encountered during the chat."
                    />
                    {variableDetails && (
                        <EmailConfigurationComponent
                            variableDetails={variableDetails}
                            saveEmailTemplate={async (emailTemplate: string) => await repository.Configuration.Email.SaveAreaFallbackEmailTemplate(intentId, emailTemplate)}
                            saveEmailSubject={async (emailSubject: string) => await repository.Configuration.Email.SaveAreaFallbackSubject(intentId, emailSubject)}
                            getCurrentEmailSubject={async () => await repository.Configuration.Email.GetAreaFallbackSubject(intentId)}
                            getCurrentEmailTemplate={async () => await repository.Configuration.Email.GetAreaFallbackEmailTemplate(intentId)}
                        />
                    )}
                </>
            )}
        </>
    );
};
