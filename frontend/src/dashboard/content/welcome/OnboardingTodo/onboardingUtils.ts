import { Todos, TodosAsBoolean } from "@Palavyr-Types";
import { every } from "lodash";

export const isNotNullOrUndefinedOrWhitespace = (val: any) => {
    return val !== null && val !== "" && val !== undefined;
};

export const convertTodos = (todos: Todos) => {
    return {
        name: isNotNullOrUndefinedOrWhitespace(todos.name),
        emailAddress: todos.emailAddress,
        logoUri: isNotNullOrUndefinedOrWhitespace(todos.logoUri),
        isVerified: todos.isVerified,
        awaitingVerification: todos.awaitingVerification,
        phoneNumber: isNotNullOrUndefinedOrWhitespace(todos.phoneNumber),
    };
};

export const allClear = (convertedTodos: TodosAsBoolean | undefined) => {
    if (convertedTodos === undefined) return false;
    const res = Object.keys(convertedTodos)
        .filter((x: string) => x !== "emailAddress")
        .map((todoKey: string) => {
            return convertedTodos[todoKey];
        });
    return every(res, Boolean);
};
