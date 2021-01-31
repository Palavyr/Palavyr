import { ApiClient } from "@api-client/Client";
import { makeStyles, Table, TableBody, TableContainer, TableHead } from "@material-ui/core";
import { AlignCenter } from "dashboard/layouts/positioning/AlignCenter";
import { isNull } from "lodash";
import React, { useCallback, useEffect, useState } from "react";
import { DoCompanyLogo } from "./DoCompanyLogo";
import { DoCompanyName } from "./DoCompanyName";
import { DoDefaultEmail } from "./DoDefaultEmail";
import { DoPhoneNumber } from "./DoPhoneNumber";

const useStyles = makeStyles((theme) => ({
    list: {
        textAlign: "left",
    },
    container: {
        width: "50%",
    },
    body: {
        textAlign: "center"
    }
}));

type Todos = {
    name: string;
    emailAddress: string;
    logoUri: string;
    isVerified: boolean;
    awaitingVerification: boolean;
    phoneNumber: string;
};

type TodosAsBoolean = {
    name: boolean;
    emailAddress: string;
    logoUri: boolean;
    isVerified: boolean;
    awaitingVerification: boolean;
    phoneNumber: boolean;
};

const isNotNullOrUndefinedOrWhitespace = (val: any) => {
    return val !== null && val !== "" && val !== undefined;
};

const convertTodos = (todos: Todos) => {
    return {
        name: isNotNullOrUndefinedOrWhitespace(todos.name),
        emailAddress: todos.emailAddress,
        logoUri: isNotNullOrUndefinedOrWhitespace(todos.logoUri),
        isVerified: todos.isVerified,
        awaitingVerification: todos.awaitingVerification,
        phoneNumber: isNotNullOrUndefinedOrWhitespace(todos.phoneNumber),
    };
};

export const OnboardingTodo = () => {
    const client = new ApiClient();
    const cls = useStyles();

    const [todos, setTodos] = useState<TodosAsBoolean>();
    const [loading, setLoading] = useState<boolean>(true);

    const loadTodos = useCallback(async () => {
        const { data: name } = await client.Settings.Account.getCompanyName();
        const { data: logoUri } = await client.Settings.Account.getCompanyLogo();
        const {
            data: { phoneNumber, locale },
        } = await client.Settings.Account.getPhoneNumber();
        var {
            data: { emailAddress, isVerified, awaitingVerification },
        } = await client.Settings.Account.getEmail();

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
        loadTodos();
        setLoading(false);
    }, []);

    return (
        <AlignCenter>
            {!loading && (
                <TableContainer className={cls.container}>
                    <Table>
                        <TableHead>Complete the following tasks:</TableHead>
                        <TableBody className={cls.body}>
                            {!todos?.isVerified && <DoDefaultEmail emailAddress={todos?.emailAddress} awaitingVerification={todos?.awaitingVerification} />}
                            {!todos?.name && <DoCompanyName />}
                            {!todos?.logoUri && <DoCompanyLogo />}
                            {!todos?.phoneNumber && <DoPhoneNumber />}
                        </TableBody>
                    </Table>
                </TableContainer>
            )}
        </AlignCenter>
    );
};
