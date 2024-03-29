import React, { useCallback, useContext, useEffect, useState } from "react";
import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { makeStyles } from "@material-ui/core";
import { ResponseVariable } from "@Palavyr-Types";
import { EmailConfigurationComponent } from "frontend/dashboard/content/responseConfiguration/uploadable/emailTemplates/EmailConfigurationComponent";
import { Align } from "@common/positioning/Align";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";

const useStyles = makeStyles<{}>(() => ({
    root: {
        width: "100%",
        height: "100%",
        marginTop: "1rem",
    },
}));

export const DefaultEmailTemplate = () => {
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();

    const [variableDetails, setVariableDetails] = useState<ResponseVariable[]>();

    const loadVariableDetails = useCallback(async () => {
        const variableDetails = await repository.Configuration.Email.GetVariableDetails();
        setVariableDetails(variableDetails);
    }, []);

    useEffect(() => {
        loadVariableDetails();
    }, [loadVariableDetails]);

    return (
        <div className={cls.root}>
            <Align>
                <HeaderStrip title="Default Email Response" subtitle="Use this editor to create an HTML email template that will be sent as the email response for this intent." />
            </Align>
            {variableDetails && (
                <EmailConfigurationComponent
                    variableDetails={variableDetails}
                    saveEmailTemplate={repository.Configuration.Email.SaveDefaultFallbackEmailTemplate}
                    saveEmailSubject={repository.Configuration.Email.SaveDefaultFallbackSubject}
                    getCurrentEmailSubject={repository.Configuration.Email.GetDefaultFallbackSubject}
                    getCurrentEmailTemplate={repository.Configuration.Email.GetDefaultFallbackEmailTemplate}
                />
            )}
        </div>
    );
};
