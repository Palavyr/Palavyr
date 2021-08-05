import * as React from "react";
import { makeStyles, Divider } from "@material-ui/core";
import { useHistory } from "react-router-dom";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { OnboardingTodo } from "./OnboardingTodo/OnboardingTodo";
import { useContext, useEffect, useState } from "react";
import { TodosAsBoolean } from "@Palavyr-Types";
import { allClear, convertTodos } from "./OnboardingTodo/onboardingUtils";
import { SessionStorage } from "localStorage/sessionStorage";
import { CacheIds } from "@api-client/AxiosClient";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { QuickStartCard } from "./quickStartGuide/QuickStartCard";

const useStyles = makeStyles((theme) => ({
    background: {
        paddingTop: "3rem",
        background: theme.palette.background.default,
    },
    wrapper: {
        position: "relative",
        height: "100%",
        textAlign: "center",
        background: theme.palette.background.default,
    },

    sectionHeadDiv: {
        width: "100%",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
    },
    listItems: {
        widgth: "40%",
        margin: "0 auto",
        padding: "1rem",
    },
    alert: {
        border: "1px solid black",
        marginTop: "1rem",
        marginBottom: "1rem",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
    },

    alertTitle: {
        fontSize: "16pt",
        fontWeight: "bold",
    },
    headCard: {
        display: "flex",
        justifyContent: "center",
    },
}));

export const QuickStartGuide = () => {
    const { repository } = useContext(DashboardContext);

    const { checkAreaCount } = React.useContext(DashboardContext);

    const [todos, setTodos] = useState<TodosAsBoolean>();
    const [loading, setLoading] = useState<boolean>(true);

    const loadTodos = React.useCallback(async () => {
        const name = await repository.Settings.Account.getCompanyName();
        const logoUri = await repository.Settings.Account.getCompanyLogo();
        const { phoneNumber, locale } = await repository.Settings.Account.getPhoneNumber();
        const { emailAddress, isVerified, awaitingVerification } = await repository.Settings.Account.getEmail();

        const todos = {
            name,
            emailAddress,
            isVerified,
            awaitingVerification,
            logoUri,
            phoneNumber,
            locale,
        };

        const todosAsBoolean = convertTodos(todos);
        setTodos(todosAsBoolean);
    }, []);

    useEffect(() => {
        SessionStorage.clearCacheValue(CacheIds.Enquiries);
        loadTodos();
        setLoading(false);
    }, []);

    return (
        <>
            {todos && !allClear(todos) && <OnboardingTodo todos={todos} />}
            <Divider />
            <AreaConfigurationHeader title="Quick Start Guide" subtitle="Follow the steps to learn about how Palavyr works and what you should do to get started" />
            <QuickStartCard
                title="1. Click to create your first area"
                content="You can check out the default area to see how you might use Palavyr. Be sure to disable or delete this area before you go live with the widget."
                checkAreaCount={checkAreaCount}
            />
            <QuickStartCard
                title="2. Configure your new area"
                content="In your area, follow the tabs in the order they are provided (from left to right). At the end, you can preview your response PDF."
                checkAreaCount={checkAreaCount}
            />

            <QuickStartCard
                title="3. Add the widget to your site"
                content="When you are ready to use your configured widget, simple paste the provided code into your website's html."
                checkAreaCount={checkAreaCount}
            />
        </>
    );
};
