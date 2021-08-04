import * as React from "react";
import { makeStyles, Card, Typography, Divider } from "@material-ui/core";
import classNames from "classnames";
import { useHistory } from "react-router-dom";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { OnboardingTodo } from "./OnboardingTodo/OnboardingTodo";
import { useContext, useEffect, useState } from "react";
import { TodosAsBoolean } from "@Palavyr-Types";
import { allClear, convertTodos } from "./OnboardingTodo/onboardingUtils";
import { SessionStorage } from "localStorage/sessionStorage";
import { CacheIds } from "@api-client/AxiosClient";

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
    sectionDiv: {
        width: "100%",
        display: "flex",
        justifyContent: "center",
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
    card: {
        width: "50%",
        margin: "4rem",
        padding: "3rem",
        color: "white",
        background: theme.palette.secondary.main,
    },
    alertTitle: {
        fontSize: "16pt",
        fontWeight: "bold",
    },

    highlight: {
        "&:hover": {
            background: theme.palette.primary.dark,
            color: theme.palette.getContrastText(theme.palette.primary.dark),
        },
    },
    headCard: {
        display: "flex",
        justifyContent: "center",
    },
    clickable: {
        "&:hover": {
            cursor: "pointer",
        },
    },
}));

export const QuickStartGuide = () => {
    const cls = useStyles();
    const history = useHistory();
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
        <div className={cls.wrapper}>
            <Card className={cls.background}>
                {todos && !allClear(todos) && <OnboardingTodo todos={todos} />}
                <Divider />
                <div className={cls.headCard}>
                    <div className={classNames("quick-start-guide")} style={{ borderBottom: "2px solid black", width: "50%" }}>
                        <Typography variant="h3" style={{ paddingTop: "2rem", paddingBottom: "2rem" }}>
                            Quick Start Guide
                        </Typography>
                        <Typography style={{ paddingTop: "2rem", paddingBottom: "2rem" }}>Follow the steps to learn about how Palavyr works and what you should do to get started.</Typography>
                    </div>
                </div>
                <div className={classNames(cls.sectionDiv)}>
                    <Card className={classNames(cls.card, cls.highlight, cls.clickable)} onClick={() => checkAreaCount()}>
                        <Typography gutterBottom variant="h4">
                            1. Click to create your first area
                        </Typography>
                        <Typography variant="body1" align="left" gutterBottom>
                            You can check out the default area to see how you might use Palavyr. Be sure to disable or delete this area before you go live with the widget.
                        </Typography>
                    </Card>
                </div>
                <div className={cls.sectionDiv}>
                    <Card className={cls.card}>
                        <Typography gutterBottom variant="h4">
                            2. Configure your new area
                        </Typography>
                        <Typography align="left">In your area, follow the tabs in the order they are provided (from left to right). At the end, you can preview your response PDF.</Typography>
                    </Card>
                </div>
                <div className={cls.sectionDiv}>
                    <Card className={classNames(cls.card, cls.highlight, cls.clickable)} onClick={() => history.push("/dashboard/getwidget")}>
                        <Typography gutterBottom variant="h4">
                            3. Add the widget to your site
                        </Typography>
                        <Typography variant="body1" align="left" gutterBottom>
                            When you are ready to use your configured widget, simple paste the provided code into your website's html.
                        </Typography>
                    </Card>
                </div>
            </Card>
        </div>
    );
};
